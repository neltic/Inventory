import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatError, MatFormField, MatHint, MatLabel } from '@angular/material/form-field';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatOption, MatSelectModule, MatSelectTrigger } from '@angular/material/select';
import { BrandForm, IBrand } from '@models';
import { BrandService } from '@services';
import { BaseFormComponent } from '../../../shared/components/base-form/base-form';
import { TranslateDirective } from '../../../shared/directives/translate-directive';
import { TranslateErrorDirective } from "../../../shared/directives/translate-error-directive";

@Component({
  selector: 'app-brand-edit-dialog',
  imports: [
    MatIcon,
    MatButtonModule,
    MatFormField,
    MatLabel,
    MatError,
    MatHint,
    MatProgressSpinnerModule,
    MatInputModule,
    MatIconModule,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatSelectModule,
    MatSelectTrigger,
    MatOption,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatChipsModule,
    MatIconModule,
    FormsModule,
    TranslateDirective,
    TranslateErrorDirective
],
  templateUrl: './brand-edit-dialog.html',
  styleUrl: './brand-edit-dialog.scss',
})
export class BrandEditDialog extends BaseFormComponent implements OnInit {
  private brandService: BrandService = inject(BrandService);
  private dialogRef: MatDialogRef<BrandEditDialog> = inject(MatDialogRef<BrandEditDialog>);
  public data: IBrand = inject(MAT_DIALOG_DATA);
   
  mainForm = this.fb.group<BrandForm>({
    name: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3), Validators.maxLength(64)] }),
    color: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(7), Validators.maxLength(7)] }),
    background: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(7), Validators.maxLength(7)] }),
    includedIn: this.fb.control([], { nonNullable: true, validators: [Validators.required, Validators.minLength(1)] })
  });

  constructor() {
    super();
  }

  ngOnInit() {
    this.initComponent(['name', 'color', 'background', 'includedIn']);
    if (this.data) {
      this.mainForm.patchValue({
        ...this.data,
        includedIn: this.entityScope.toArray(this.data.includedIn)
      });      
    }
  }  

  save() {
    this.isSaving.set(true);    
    const formValues = this.mainForm.getRawValue();    
    const brandToSave: IBrand = {
      ...this.data,
      ...formValues,
      includedIn: this.entityScope.toScope(formValues.includedIn || [])
    };    
    this.brandService.save(brandToSave).subscribe({
      next: () => this.dialogRef.close(true),
      complete: () => this.isSaving.set(false),
      error: (error) => {
          this.handleError(error);
          this.isSaving.set(false);
        }
    }); 
  }
  
}

