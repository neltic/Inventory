import { Component, computed, inject, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TranslateDirective } from '../../directives/translate-directive';
import { BaseFormComponent } from '../base-form/base-form';

@Component({
    selector: 'app-basic-actions',
    standalone: true,
    imports: [MatButtonModule, MatIconModule, MatProgressSpinnerModule, TranslateDirective],
    templateUrl: './basic-actions.html',
    styleUrl: './basic-actions.scss',
})
export class BasicActions {
    protected parent = inject(BaseFormComponent);

    saveDisabledWhen = input.required<boolean>();

    disabled = computed(() => {
        return this.parent.isBusy() || this.saveDisabledWhen();
    });
}
