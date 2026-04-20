import { Component, effect, inject, OnInit, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule, Validators, ɵInternalFormsSharedModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from "@angular/material/icon";
import { MatError, MatFormField, MatHint, MatInputModule, MatLabel } from "@angular/material/input";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BoxForm, IBox } from '@models';
import { BoxService, BrandService, CategoryService, FileService } from '@services';
import { finalize, switchMap } from 'rxjs';
import { BaseFormComponent } from '../../../shared/components/base-form/base-form';
import { BrandSelect } from '../../../shared/components/brand-select/brand-select';
import { CategorySelect } from '../../../shared/components/category-select/category-select';
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';
import { TranslateDirective } from '../../../shared/directives/translate-directive';
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { RelativeTimePipe } from '../../../shared/pipes/relative-time-pipe';
import { TranslateErrorPipe } from '../../../shared/pipes/translate-error-pipe';
import { TranslatePipe } from '../../../shared/pipes/translate-pipe';
import { BoxBreadcrumb } from '../box-breadcrumb/box-breadcrumb';

@Component({
  selector: 'app-box-edit',
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
    ReactiveFormsModule,
    ɵInternalFormsSharedModule,
    ImgFallbackDirective,
    RelativeTimePipe,
    BoxBreadcrumb,
    CategorySelect,
    BrandSelect,
    AsPhotoPipe,
    TranslatePipe,
    TranslateDirective,
    TranslateErrorPipe
  ],
  providers: [{ provide: BaseFormComponent, useExisting: BoxEdit }],
  templateUrl: './box-edit.html',
  styleUrl: './box-edit.scss',
})
export class BoxEdit extends BaseFormComponent implements OnInit {
  private boxService = inject(BoxService);
  private fileService = inject(FileService);  
  protected categoryService = inject(CategoryService);
  protected brandService = inject(BrandService);  
  protected imageGuid = signal<string | null>(null);
  
  boxResource = rxResource<IBox, any>({
    stream: () => { 
      return this.boxService.getBoxBy(this.params.boxId()); 
    }
  });

  mainForm = this.fb.group<BoxForm>({
    parentBoxId: this.fb.control(this.params.parentBoxId()),
    name: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(5), Validators.maxLength(64)] }),
    categoryId: this.fb.control(0, { nonNullable: true, validators: [Validators.required, this.categoryService.validators.existsInScope(this.EntityScope.Box)] }),
    brandId: this.fb.control(0, { nonNullable: true, validators: [Validators.required, this.brandService.validators.existsInScope(this.EntityScope.Box)] }),
    notes: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(10), Validators.maxLength(512)] }),
    height: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
    width: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
    depth: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] })
  });

  constructor() { 
    super();
    effect(() => {
      const box = this.boxResource.value();      
      if (box && !this.mainForm.dirty) {        
        this.mainForm.patchValue(box);
      }
    });
  }

  ngOnInit() {
    this.initComponent(['name', 'brandId', 'categoryId', 'width', 'height', 'depth', 'notes']);
  }

  onSubmit(): void {
    if (this.mainForm.invalid) {
      this.mainForm.markAllAsTouched();
      return;
    }
    if (this.isSaving()) return;
    if (this.isUploadingImage()) {
      this.openSnack('warning', 'Global.OK', 'Message.WAIT_IMAGE_UPLOAD');
      return;
    }
    this.isSaving.set(true);

    const currentId = this.boxResource.value()?.boxId ?? 0;
    const formValues = this.mainForm.getRawValue();

    const boxData: IBox = {
      ...formValues,
      boxId: currentId
    };

    this.boxService.saveBox(boxData)
      .pipe(    
        finalize(() => this.isSaving.set(false)) 
      )
      .subscribe({
        next: () => {
          this.openSnack('success', 'Global.OK', 'Message.BOX_SAVED');
          this.mainForm.markAsPristine();   
        },
        error: (error) => this.handleError(error)
      });
  } 
  
  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const currentId = this.boxResource.value()?.boxId ?? 0;

    if (currentId === 0 || !input.files?.length) return;
  
    const file = input.files[0];      
    this.isUploadingImage.set(true);
    
    this.fileService.uploadTempImage(file).pipe(
      switchMap(res => {
        this.imageGuid.set(res.fileGuid);        
        return this.boxService.assignImage(currentId, res.fileGuid);
      }), finalize(() => {
        input.value = '';
        this.onImageLoad();
      })
    ).subscribe({
      next: (response: any) => {
        const newDate = response.updatedAt;
        const currentBox = this.boxResource.value();
        if (currentBox) {
            this.boxResource.set({
                ...currentBox,
                updatedAt: newDate
            });
        }
        this.openSnack('success', 'Global.OK', 'Message.IMAGE_UPDATED');
      },
      error: (error) => this.handleError(error, 'Error.IMAGE_PROCESSING')
    });
  }  

}
