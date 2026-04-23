import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { PreloadAllModules, provideRouter, withComponentInputBinding, withPreloading, withRouterConfig } from '@angular/router';
import { GlobalizationService } from '@services';
import { INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG, includeBearerTokenInterceptor, provideKeycloak } from 'keycloak-angular';
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
    {
      provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
      useValue: [
        {
          urlPattern: /http:\/\/localhost:5000\/.*/i,
          httpMethods: ['GET', 'POST', 'PUT', 'DELETE']
        }
      ]
    },
    provideHttpClient(
      withInterceptors([
        languageInterceptor,
        includeBearerTokenInterceptor
      ])
    ),
    provideKeycloak({
      config: {
        url: 'http://localhost:8080',
        realm: 'StockRealm',
        clientId: 'stock-frontend'
      },
      initOptions: {
        onLoad: 'check-sso',
        silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
        checkLoginIframe: false
      }
    }),
    provideAppInitializer(() => {
        const globalization = inject(GlobalizationService);
        return globalization.initializeApp();
    }),
  ]
};
