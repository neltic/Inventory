import { Signal } from '@angular/core';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class InListValidator {
  
  static exists<T>(dataSignal: Signal<T[]>, idKey: keyof T, minId = 0): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const id = control.value;
      const list = dataSignal();
      
      const exists = list.some(item => item[idKey] === id);
      
      return (exists && id >= minId) ? null : { entityNotFound: true };
    };
  }
}