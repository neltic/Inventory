import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { PreloadAllModules, provideRouter, withComponentInputBinding, withPreloading, withRouterConfig } from '@angular/router';
import { GlobalizationService } from '@services';
import { routes } from './app.routes';
import { languageInterceptor } from './core/interceptors/language.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes, 
      withComponentInputBinding(),
      withRouterConfig({ 
        paramsInheritanceStrategy: 'always',
        onSameUrlNavigation: 'reload'
      }),
      withPreloading(PreloadAllModules)
    ),
    provideHttpClient(
      withInterceptors([
        languageInterceptor
      ])
    ),
    provideAppInitializer(() => {
        const globalization = inject(GlobalizationService);
        return globalization.initializeApp();
    }),
  ]
};
