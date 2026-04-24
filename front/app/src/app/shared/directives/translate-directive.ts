import { Directive, effect, ElementRef, inject, Input, Renderer2, signal } from '@angular/core';
import { GlobalizationKey } from '@core';
import { GlobalizationService } from '@services';

@Directive({
  selector: '[translate]',
  standalone: true
})
export class TranslateDirective {

  private globalization = inject(GlobalizationService);
  private el = inject(ElementRef);
  private renderer = inject(Renderer2);  
  private key = signal<GlobalizationKey | null>(null);
  private params = signal<any[]>([]);
  
  @Input('translate') set translateKey(val: GlobalizationKey) { this.key.set(val); }
  @Input('translateParams') set translateParams(val: any[]) { this.params.set(val || []); }

  constructor() {
    effect(() => {
      const currentKey = this.key();
      const currentParams = this.params();      
      this.globalization.translations();
      if (currentKey) {
        this.renderer.setProperty(this.el.nativeElement, 'textContent', this.globalization.translate(currentKey, currentParams));
      }
    });
  }

}