import { HttpInterceptorFn } from '@angular/common/http';

const DEFAULT_LANG = 'en';

export const languageInterceptor: HttpInterceptorFn = (req, next) => {

    const lang = localStorage.getItem('language') || DEFAULT_LANG;

    if (req.url.includes('api/')) {
        const clonedReq = req.clone({
            setHeaders: { 'Accept-Language': lang }
        });
        return next(clonedReq);
    }

    return next(req);
};