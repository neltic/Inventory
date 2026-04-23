import { Component, Input, OnInit, Signal, WritableSignal, inject, signal } from '@angular/core';
import { ControlContainer, FormGroup } from '@angular/forms';
import { EntityScope } from '@models';
import { GlobalizationKey } from '../../../core/types/globalization-keys';
import { BaseFormComponent } from '../base-form/base-form';

@Component({ template: '' })
export abstract class BaseSelectComponent<T> implements OnInit {
  
  @Input({ required: true }) controlName!: string;
  @Input({ required: true }) label!: GlobalizationKey;
  @Input({ required: true }) friendlyErrorName!: GlobalizationKey;
  @Input() scope: EntityScope = EntityScope.None;  
  
  private parentComponent = inject(BaseFormComponent, { optional: true });  
  protected container = inject(ControlContainer);
  public catalogList: Signal<T[]> = signal([]);
  public parentErrorSignal?: WritableSignal<string>;

  abstract initList(): void;

  ngOnInit() {
    this.initList();
    if (this.parentComponent && this.parentComponent.errorMessages) {
      this.parentErrorSignal = this.parentComponent.errorMessages[this.controlName];
    }
  }

  get parentForm(): FormGroup {
    return this.container.control as FormGroup;
  }

  handleBlur() {        
    if (this.parentComponent) {
      this.parentComponent.updateErrorMessage(
        this.controlName, 
        this.friendlyErrorName
      );
    } else {
      console.error('Use { provide: BaseFormComponent, useExisting: ??? }');
    }
  }
}