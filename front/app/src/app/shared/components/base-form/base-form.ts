import { Directive, Injector, Signal, WritableSignal, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { AbstractControl, FormArray, FormBuilder, FormControlStatus, FormGroup, PristineChangeEvent, StatusChangeEvent } from '@angular/forms';
import { GlobalizationKey } from '@core';
import { filter, map, startWith } from 'rxjs';
import { BaseComponent } from '../base/base';
import { ERROR_FORM_MESSAGES } from '../error/error-form-mapping';

@Directive()
export abstract class BaseFormComponent extends BaseComponent {

    private injector = inject(Injector);
    protected fb = inject(FormBuilder);
    public isSaving = signal(false);
    public isUploadingImage = signal(false);
    public errorMessages: Record<string, WritableSignal<string>> = {};
    public formState!: Signal<{ status: FormControlStatus; pristine: boolean }>;

    public abstract mainForm: FormGroup;

    protected initComponent(fields: string[]) {
        this.setErrorMessages(fields);
        this.formState = toSignal(
            this.mainForm.events.pipe(
                filter(e => e instanceof StatusChangeEvent || e instanceof PristineChangeEvent),
                startWith(null),
                map(() => ({
                    status: this.mainForm.status,
                    pristine: this.mainForm.pristine
                }))
            ),
            {
                initialValue: { status: this.mainForm.status, pristine: this.mainForm.pristine },
                injector: this.injector
            }
        );
    }

    public isFormValid = computed(() => this.formState().status === 'VALID');

    public isFormPristine = computed(() => this.formState().pristine);

    public isFormDirty = computed(() => !this.formState().pristine);

    protected setErrorMessages(fields: string[]) {
        fields.forEach(field => {
            this.errorMessages[field] = signal('');
        });
    }

    public isBusy = computed(() => {
        return this.isSaving() || this.isUploadingImage();
    });

    updateErrorMessage(fieldName: string, friendlyErrorName: GlobalizationKey) {
        const control = this.findControlRecursive(this.mainForm, fieldName);
        const errorSignal = this.errorMessages[fieldName];
        if (!control || !errorSignal) return;
        const firstErrorKey = Object.keys(control.errors || {})[0];
        const labelKey = ERROR_FORM_MESSAGES[firstErrorKey]
        if (firstErrorKey && labelKey) {
            errorSignal.set(`${labelKey}|${friendlyErrorName}`);
        } else {
            errorSignal.set('');
        }
    }

    private findControlRecursive(parent: AbstractControl, name: string): AbstractControl | null {
        if (parent instanceof FormGroup) {
            if (parent.controls[name]) return parent.controls[name];
            for (const key in parent.controls) {
                const found = this.findControlRecursive(parent.controls[key], name);
                if (found) return found;
            }
        } else if (parent instanceof FormArray) {
            for (const control of parent.controls) {
                const found = this.findControlRecursive(control, name);
                if (found) return found;
            }
        }
        return null;
    }

    public abstract onSave(): void | Promise<void>;

    async onCancel(): Promise<void> {
        if (this.mainForm.dirty) {
            const confirmed = await this.openInfo('Message.PENDING_CHANGES');
            if (!confirmed) {
                return;
            }
        }
        this.goBack();
    }

    onImageLoad() {
        this.isUploadingImage.set(false);
    }
}