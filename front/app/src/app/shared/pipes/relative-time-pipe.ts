import { Pipe, PipeTransform } from '@angular/core';

const DEFAULT_LANG = 'en';

@Pipe({
    name: 'relativeTime',
    standalone: true,
    pure: false
})
export class RelativeTimePipe implements PipeTransform {

    transform(value: string | Date | undefined): string {
        if (!value) return 'Undated';

        const date = new Date(value);
        const now = new Date(Math.floor(Date.now() / 30000) * 30000);
        const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

        if (isNaN(date.getTime())) return 'Invalid date';

        const lang = localStorage.getItem('language') || DEFAULT_LANG;

        const formatter = new Intl.RelativeTimeFormat(lang, { numeric: 'auto' });

        if (diffInSeconds < 60) return 'a moment ago';

        const minutes = Math.floor(diffInSeconds / 60);
        if (minutes < 60) return formatter.format(-minutes, 'minutes');

        const hours = Math.floor(minutes / 60);
        if (hours < 24) return formatter.format(-hours, 'hours');

        const days = Math.floor(hours / 24);
        if (days < 30) return formatter.format(-days, 'days');

        const months = Math.floor(days / 30);
        if (months < 12) return formatter.format(-months, 'months');

        const years = Math.floor(days / 365);
        return formatter.format(-years, 'years');
    }
}