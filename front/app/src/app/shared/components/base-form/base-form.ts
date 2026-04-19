import { Directive, Injector, Signal, WritableSignal, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { AbstractControl, FormArray, FormBuilder, FormControlStatus, FormGroup } from '@angular/forms';
import { startWith } from 'rxjs';
import { BaseComponent } from '../base/base';
import { ERROR_FORM_MESSAGES } from '../error/error-form-mapping';

@Directive()
export abstract class BaseFormComponent extends BaseComponent {

    private injector = inject(Injector);  
    protected fb = inject(FormBuilder);    
    public isSaving = signal(false);
    public isUploadingImage = signal(false);
    public errorMessages: Record<string, WritableSignal<string>> = {};    
    public formStatus!: Signal<FormControlStatus>; 

    public abstract mainForm: FormGroup;

    protected initComponent(fields: string[]) {
        this.setErrorMessages(fields);
        this.formStatus = toSignal(
            this.mainForm.statusChanges.pipe(
                startWith(this.mainForm.status)), 
                { 
                    initialValue: this.mainForm.status as FormControlStatus,
                    injector: this.injector 
                }
        );
    }

    protected setErrorMessages(fields: string[]) {
        fields.forEach(field => {
            this.errorMessages[field] = signal('');
        });
    }

    public isBusy = computed(() => {
        if (!this.formStatus) return true;
        return this.isSaving() || this.isUploadingImage() || this.formStatus() === 'INVALID';
    });   
  
    updateErrorMessage(fieldName: string, friendlyErrorName: string) {        
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

    async onCancel(): Promise<void> {
        if (this.mainForm.dirty) {
            const confirmed = await this.openInfo('You have unsaved changes. Are you sure you want to exit?');
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