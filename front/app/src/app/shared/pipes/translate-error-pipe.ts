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
      translatedFriendlyName = this.globalization.translate(friendlyNamePart);
    } else {
      translatedFriendlyName = friendlyNamePart || '';
    }
    
    if (errorKeyPart && errorKeyPart.includes('.')) {      
      return this.globalization.translate(errorKeyPart, [translatedFriendlyName]);
    }

    return value;
  }
}
