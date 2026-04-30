import { Component, signal } from '@angular/core';

@Component({
    selector: 'app-footer',
    imports: [],
    templateUrl: './footer.html',
    styleUrl: './footer.scss',
})
export class Footer {
    protected readonly currentDate = signal(new Date());
}
