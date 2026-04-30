import { Component, inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatError, MatFormField, MatHint, MatLabel } from '@angular/material/form-field';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatOption, MatSelectModule, MatSelectTrigger } from '@angular/material/select';
import { CategoryForm, ICategory } from '@models';
import { CategoryService } from '@services';
import { BaseFormComponent } from '../../../shared/components/base-form/base-form';
import { TranslateDirective } from '../../../shared/directives/translate-directive';
import { TranslateErrorDirective } from '../../../shared/directives/translate-error-directive';

@Component({
    selector: 'app-category-edit-dialog',
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
        MatIconModule,
        FormsModule,
        TranslateDirective,
        TranslateErrorDirective
    ],
    templateUrl: './category-edit-dialog.html',
    styleUrl: './category-edit-dialog.scss',
})
export class CategoryEditDialog extends BaseFormComponent implements OnInit {
    public data: ICategory = inject(MAT_DIALOG_DATA);
    private categoryService: CategoryService = inject(CategoryService);
    private dialogRef: MatDialogRef<CategoryEditDialog> = inject(MatDialogRef<CategoryEditDialog>);

    mainForm = this.fb.group<CategoryForm>({
        name: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3), Validators.maxLength(64)] }),
        icon: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3), Validators.maxLength(32)] }),
        color: this.fb.control('', { nonNullable: true, validators: [Validators.required, Validators.minLength(7), Validators.maxLength(7)] }),
        includedIn: this.fb.control([], { nonNullable: true, validators: [Validators.required, Validators.minLength(1)] })
    });

    constructor() {
        super();
        if (!this.securityService.hasRole(this.Role.Editor)) {
            this.dialogRef.close();
        }
    }

    ngOnInit() {
        this.initComponent(['name', 'icon', 'color', 'includedIn']);
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
        const categoryToSave: ICategory = {
            ...this.data,
            ...formValues,
            includedIn: this.entityScope.toScope(formValues.includedIn || [])
        };

        this.categoryService.save(categoryToSave).subscribe({
            next: () => this.dialogRef.close(true),
            complete: () => this.isSaving.set(false),
            error: (error) => {
                this.handleError(error);
                this.isSaving.set(false);
            }
        });
    }

}
