import { inject, Pipe, PipeTransform } from '@angular/core';
import { GlobalizationService } from '@services';

@Pipe({
  name: 'translateError',
  standalone: true,
  pure: false
})
export class TranslateErrorPipe implements PipeTransform {

  private globalization = inject(GlobalizationService);

  transform(value: string | null | undefined): string {
    if (!value) return '';

    const [errorKeyPart, friendlyNamePart] = value.split('|');
    
    let translatedFriendlyName = '';
    if (friendlyNamePart && friendlyNamePart.includes('.')) {
      const [fContext, fKey] = friendlyNamePart.split('.');
      translatedFriendlyName = this.globalization.translate(fContext, fKey);
    } else {
      translatedFriendlyName = friendlyNamePart || '';
    }
    
    if (errorKeyPart && errorKeyPart.includes('.')) {
      const [eContext, eKey] = errorKeyPart.split('.');
      return this.globalization.translate(eContext, eKey, [translatedFriendlyName]);
    }

    return value;
  }
}
