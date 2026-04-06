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
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { RelativeTimePipe } from '../../../shared/pipes/relative-time-pipe';

@Component({
  selector: 'app-item-edit',
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
    RelativeTimePipe,
    CategorySelect,
    AsPhotoPipe
  ],
  providers: [{ provide: BaseFormComponent, useExisting: ItemEdit }],
  templateUrl: './item-edit.html',
  styleUrl: './item-edit.scss',
})
export class ItemEdit extends BaseFormComponent implements OnInit {
  private itemService: ItemService = inject(ItemService);
  private fileService: FileService = inject(FileService);
  protected categoryService: CategoryService = inject(CategoryService);
  
  imageGuid = signal<string | null>(null);
    
  itemResource = rxResource<IItem, any>({
    stream: () => { 
      return this.itemService.getItemBy(this.params.itemId()); 
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
      if (item && !this.mainForm.dirty) {        
        this.mainForm.patchValue(item);
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
      itemId: currentId
    };

    this.itemService.saveItem(itemData)
      .pipe(    
        finalize(() => this.isSaving.set(false)) 
      )
      .subscribe({
        next: () => {
          this.openSnack('success', '¡Item saved!', 'Ok');
          this.mainForm.markAsPristine();   
        },
        error: (error) => this.handleError(error)
      });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const currentId = this.itemResource.value()?.itemId ?? 0;

    if (currentId === 0 || !input.files?.length) return;
  
    const file = input.files[0];      
    this.isUploadingImage.set(true);
    
    this.fileService.uploadTempImage(file).pipe(
      switchMap(res => {
        this.imageGuid.set(res.fileGuid);        
        return this.itemService.assignImage(currentId, res.fileGuid);
      }), finalize(() => {
        input.value = '';
        this.onImageLoad();
      })
    ).subscribe({
      next: (response: any) => {
        const newDate = response.updatedAt;
        const currentItem = this.itemResource.value();
        if (currentItem) {
            this.itemResource.set({
                ...currentItem,
                updatedAt: newDate
            });
        }
        this.openSnack('success', 'Image updated!', 'Ok');
      },
      error: (error) => this.handleError(error, 'Image processing error')
    });
  }

}
