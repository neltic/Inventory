
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Entity, EventName, IAudit } from '@models';
import { AuditService } from '@services';
import { TranslateDirective } from '../../directives/translate-directive';
import { RelativeTimePipe } from '../../pipes/relative-time-pipe';

export interface AuditDialogData {
    entity: Entity;
    recordId: string;
}

@Component({
    selector: 'app-audit-dialog',
    standalone: true,
    imports: [
        CommonModule,
        MatButtonModule,
        MatDialogContent,
        MatDialogActions,
        MatDialogClose,
        MatProgressSpinnerModule,
        TranslateDirective,
        RelativeTimePipe
    ],
    templateUrl: './audit-dialog.html',
    styleUrl: './audit-dialog.scss',
})
export class AuditDialog {
    private auditService = inject(AuditService);
    readonly dialogRef = inject(MatDialogRef<AuditDialog>);
    readonly data = inject<AuditDialogData>(MAT_DIALOG_DATA);
    readonly EventName = EventName;

    historyList = rxResource<IAudit[], any>({
        stream: () => this.auditService.getHistory(this.data.entity, this.data.recordId)
    });
}