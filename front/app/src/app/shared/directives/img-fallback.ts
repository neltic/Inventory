import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
    selector: 'img[appImgFallback]',
    standalone: true
})
export class ImgFallbackDirective {

    private _fallback: string = '/img/broken-image.png';

    @Input() set appImgFallback(value: string) {
        if (value) {
            this._fallback = value;
        }
    }

    constructor(private eRef: ElementRef) { }

    @HostListener('error')
    loadFallback() {
        const element: HTMLImageElement = this.eRef.nativeElement;
        if (element.src.includes(this._fallback)) return;
        element.src = this._fallback;
    }
}