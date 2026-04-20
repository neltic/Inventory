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
    return this.globalization.translate(value, params);
  }

}
