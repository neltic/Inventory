import { HttpInterceptorFn } from '@angular/common/http';

export const languageInterceptor: HttpInterceptorFn = (req, next) => {

    const lang = 'es-MX';

    const languageReq = req.clone({
        headers: req.headers.set('Accept-Language', lang),
    });

    return next(languageReq);
};