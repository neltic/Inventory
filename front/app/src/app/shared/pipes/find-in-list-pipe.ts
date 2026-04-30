import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'findInList',
    standalone: true
})
export class FindInListPipe implements PipeTransform {

    transform<T>(value: any, list: T[] | null | undefined, key: keyof T): T | any {
        if (value === undefined || value === null || !list) return undefined;
        return list.find(item => item[key as keyof T] === value);
    }

}
