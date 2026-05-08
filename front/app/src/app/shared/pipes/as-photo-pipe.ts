import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'asPhoto',
    standalone: true
})
export class AsPhotoPipe implements PipeTransform {

    transform(
        id: number,
        context: 'item' | 'box',
        imageAt?: string | Date,
        type: string = 'original'
    ): string {

        if (!id || id <= 0) return `/cdn/img/${context}/0.png`;

        let version = '';
        if (imageAt) {
            version = `?v=${new Date(imageAt).getTime()}`;
        }

        return `/cdn/img/${context}/${type}/${id}.png${version}`;
    }
}