import { inject, Pipe, PipeTransform } from '@angular/core';
import { GlobalizationService } from '@services';

@Pipe({
  name: 'translate',
  standalone: true,
  pure: false
})
export class TranslatePipe implements PipeTransform {

  private globalization = inject(GlobalizationService);
  
  transform(value: string, ...params: any[]): string {
    if (!value) return '';
    const parts = value.split('.');    
    if (parts.length < 2) {
      return value; 
    }
    const context = parts[0];
    const key = parts[1];
    
    return this.globalization.translate(context, key, params);
  }

}
