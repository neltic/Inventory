import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { PreloadAllModules, provideRouter, withComponentInputBinding, withPreloading, withRouterConfig } from '@angular/router';
import { routes } from './app.routes';

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
    provideHttpClient()
  ]
};
