import { A11yModule } from '@angular/cdk/a11y';
import { Component, inject, OnInit } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckbox, MatCheckboxModule } from '@angular/material/checkbox';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatError } from '@angular/material/form-field';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { BoxTransferForm, IBox, IBoxTransfer } from '@models';
import { BoxService } from '@services';
import { BaseFormComponent } from '../../../shared/components/base-form/base-form';
import { TranslateDirective } from '../../../shared/directives/translate-directive';
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { TranslateErrorPipe } from '../../../shared/pipes/translate-error-pipe';

@Component({
  selector: 'app-box-parent-select-dialog',
  imports: [
    MatIcon,
    MatButtonModule,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    ReactiveFormsModule,    
    MatIconModule,
    MatListModule,
    MatError,
    MatCheckbox,
    MatCheckboxModule,
    A11yModule,
    FormsModule,
    AsPhotoPipe,
    TranslateDirective,
    TranslateErrorPipe
  ],
  templateUrl: './box-parent-select-dialog.html',
  styleUrl: './box-parent-select-dialog.scss',
})
export class BoxParentSelectDialog extends BaseFormComponent implements OnInit {
  public data: IBox = inject(MAT_DIALOG_DATA);
  private boxService: BoxService = inject(BoxService);
  private dialogRef: MatDialogRef<BoxParentSelectDialog> = inject(MatDialogRef<BoxParentSelectDialog>);

  constructor() {
    super();
  }

  mainForm = this.fb.group<BoxTransferForm>({
    newParentId: this.fb.control([-1], [ this.boxService.validators.isValidDestination() ]),
    confirmMove: this.fb.control(false, { nonNullable: true, validators: [Validators.requiredTrue]})
  });

  boxResourceList = rxResource<IBoxTransfer[], any>({    
    stream: () => this.boxService.getAvailableParentBoxesBy(this.data.boxId)
  });

  ngOnInit(): void {
    this.initComponent(['newParentId', 'confirmMove']);
  }

  move() {
    this.isSaving.set(true);    
    const formValues = this.mainForm.getRawValue();      
    const newParentId = Array.isArray(formValues.newParentId) ? formValues.newParentId[0] : formValues.newParentId;
    this.boxService.moveBox(this.data.boxId, newParentId).subscribe({
      next: () => this.dialogRef.close(true),
      complete: () => this.isSaving.set(false)
    });
  }

  get canMove(): boolean {
    return this.mainForm.valid;
  }
}
