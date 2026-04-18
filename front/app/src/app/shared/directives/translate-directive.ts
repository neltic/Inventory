import { Directive, effect, ElementRef, inject, Input, signal } from '@angular/core';
import { GlobalizationService } from '@services';

@Directive({
  selector: '[translate]',
  standalone: true
})
export class TranslateDirective {

  private globalization = inject(GlobalizationService);
  private el = inject(ElementRef);
  
  private key = signal<string | null>(null);
  private params = signal<any[]>([]);

  @Input('translate') set translateKey(val: string) { this.key.set(val); }
  @Input('translateParams') set translateParams(val: any[]) { this.params.set(val || []); }

  constructor() {
    effect(() => {
      const currentKey = this.key();
      const currentParams = this.params();      
      this.globalization.translations();
      if (currentKey) {
        this.el.nativeElement.textContent = this.translate(currentKey, currentParams);
      }
    });
  }

  private translate(value: string, params: any[]): string {
    const [context, key] = value.split('.');
    return key ? this.globalization.translate(context, key, params) : value;
  }
}