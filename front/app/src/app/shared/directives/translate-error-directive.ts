import { Directive, effect, ElementRef, inject, Input, Renderer2, signal } from '@angular/core';
import { GlobalizationService } from '@services';

@Directive({
    selector: '[translateError]',
    standalone: true
})
export class TranslateErrorDirective {

    private globalization = inject(GlobalizationService);
    private el = inject(ElementRef);
    private renderer = inject(Renderer2);
    private value = signal<string>('');

    @Input('translateError') set translateErrorValue(val: string) { this.value.set(val); }

    constructor() {
        effect(() => {
            if (!this.value) {
                this.updateText('');
                return;
            };

            const [errorKeyPart, friendlyNamePart] = this.value().split('|');

            let translatedFriendlyName = '';
            if (friendlyNamePart && friendlyNamePart.includes('.')) {
                translatedFriendlyName = this.globalization.translate(friendlyNamePart);
            } else {
                translatedFriendlyName = friendlyNamePart || '';
            }

            if (errorKeyPart && errorKeyPart.includes('.')) {
                const finalTranslation = this.globalization.translate(errorKeyPart, [translatedFriendlyName]);
                this.updateText(finalTranslation);
                return;
            }

            this.updateText(this.value());
            return;
        });
    }

    private updateText(text: string): void {
        this.renderer.setProperty(this.el.nativeElement, 'textContent', text);
    }

}