import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule, Validators, ɵInternalFormsSharedModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon, MatIconModule } from "@angular/material/icon";
import { MatError, MatFormField, MatHint, MatInputModule, MatLabel } from "@angular/material/input";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { IItem, ItemForm } from '@models';
import { CategoryService, FileService, ItemService } from '@services';
import { finalize, switchMap } from 'rxjs';
import { BaseFormComponent } from '../../../shared/components/base-form/base-form';
import { CategorySelect } from '../../../shared/components/category-select/category-select';
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';

@Component({
  selector: 'app-item-new',
  standalone: true,
  imports: [
    MatIcon,
    MatCardModule,
    MatButtonModule,
    MatFormField,
    MatLabel,
    MatError,
    MatHint,
    MatProgressSpinnerModule,
    MatInputModule,
    MatIconModule,     
    ReactiveFormsModule,
    ɵInternalFormsSharedModule,
    ImgFallbackDirective,
    CategorySelect
  ],
  providers: [{ provide: BaseFormComponent, useExisting: ItemNew }],
  templateUrl: './item-new.html',
  styleUrl: './item-new.scss',
})
export class ItemNew extends BaseFormComponent implements OnInit {
  private fileService: FileService = inject(FileService);
  public itemService: ItemService = inject(ItemService);
  public categoryService: CategoryService = inject(CategoryService);
  public imageGuid = signal<string>('');
  
  itemResource = rxResource<IItem, any>({
    stream: () => { 
      return this.itemService.getEmptyItem();
    }
  });

  mainForm = this.fb.group<ItemForm>({
    name: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(5), Validators.maxLength(64)] }),
    categoryId: this.fb.control(0, { nonNullable: true, validators: [Validators.required, this.categoryService.validators.existsInScope(this.EntityScope.Item)] }),
    notes: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(10), Validators.maxLength(512)] }),      
  }); 

  constructor() {
    super();
    effect(() => {
      const item = this.itemResource.value();      
      if (item) {        
        this.mainForm.patchValue({
          name: item.name,
          notes: item.notes,
          categoryId: item.categoryId
        });
      }
    });
  }

  ngOnInit() {
    this.initComponent(['name', 'categoryId', 'notes']);
  }

  onSubmit(): void {
    if (this.mainForm.invalid) {
      this.mainForm.markAllAsTouched();
      return;
    }
    if (this.isSaving()) return;
    if (this.isUploadingImage()) {
      this.openSnack('warning','Please wait until the image upload finishes.', 'Ok');
      return;
    }
    this.isSaving.set(true);

    const currentId = this.itemResource.value()?.itemId ?? 0;
    const formValues = this.mainForm.getRawValue();

    const itemData: IItem = {
      ...formValues,
      itemId: currentId,
    };

    this.itemService.saveItem(itemData)
    .pipe(    
      switchMap((res: any) => {        
        return this.itemService.assignImage(res.itemId, this.imageGuid());
      })
    )
    .subscribe({
      next: () => {        
        const snackRef = this.openSnack('success','¡Item saved!', 'Ok');
        snackRef.afterDismissed().subscribe(() => {
          this.goBack();
        });
      },
      error: (error) => {
        this.handleError(error);
        this.isSaving.set(false);
      }
    });
  }  
  
  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;    
    if (!input.files?.length) return;

    const file = input.files[0];      
    this.isUploadingImage.set(true);    
    this.fileService.uploadTempImage(file).pipe(
      finalize(() => {
        input.value = '';
        this.onImageLoad();
      })
    ).subscribe({
      next: (response: any) => {
        this.imageGuid.set(response.fileGuid);        
        this.openSnack('success','Image uploaded, you can now save it!', 'Ok');
      },
      error: (error) => this.handleError(error)
    });
  }  

}
