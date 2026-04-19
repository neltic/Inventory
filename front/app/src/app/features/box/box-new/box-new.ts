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
import { TranslateErrorPipe } from '../../../shared/pipes/translate-error-pipe';
import { TranslatePipe } from '../../../shared/pipes/translate-pipe';
import { BoxBreadcrumb } from '../box-breadcrumb/box-breadcrumb';

@Component({
  selector: 'app-box-new',
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
    BoxBreadcrumb,
    CategorySelect,
    BrandSelect,
    TranslateDirective,
    TranslateErrorPipe,
    TranslatePipe
  ],
  providers: [{ provide: BaseFormComponent, useExisting: BoxNew }],
  templateUrl: './box-new.html',
  styleUrl: './box-new.scss',
})
export class BoxNew extends BaseFormComponent implements OnInit {  
  private fileService: FileService = inject(FileService);
  private categoryService: CategoryService = inject(CategoryService);
  private brandService: BrandService = inject(BrandService);
  public boxService: BoxService = inject(BoxService);
  public imageGuid = signal<string>('');
  
  boxResource = rxResource<IBox, any>({
    stream: () => { 
      return this.boxService.getEmptyBoxBy(this.params.parentBoxId());
    }
  });
  
  mainForm = this.fb.group<BoxForm>({
    parentBoxId: this.fb.control(this.params.parentBoxId()),
    name: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(5), Validators.maxLength(64)] }),
    categoryId: this.fb.control(0, { nonNullable: true, validators: [Validators.required, this.categoryService.validators.existsInScope(this.EntityScope.Box)] }),
    brandId: this.fb.control(-1, { nonNullable: true, validators: [Validators.required, this.brandService.validators.existsInScope(this.EntityScope.Box)] }),
    notes: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(10), Validators.maxLength(512)] }),
    height: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
    width: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
    depth: this.fb.control(0, { nonNullable: true, validators: [Validators.required, Validators.min(1)] })
  });

  constructor() {
    super();
    effect(() => {
      const box = this.boxResource.value();      
      if (box) {        
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
      this.openSnack('warning', 'Please wait until the image upload finishes.', 'Ok');
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
      switchMap((res: any) => {        
        return this.boxService.assignImage(res.boxId, this.imageGuid());
      }),      
    )
    .subscribe({
      next: () => {
        const snackRef = this.openSnack('success', '¡Box saved!', 'Ok');
        snackRef.afterDismissed().subscribe(() => {
          this.goBack();
        });
      },
      error: (error) => {
        this.handleError(error)
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
      error: (error) => this.handleError(error, 'Image uploading error')
    });
  }
  
}