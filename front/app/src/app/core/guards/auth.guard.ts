import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import Keycloak from 'keycloak-js';

export const authGuard: CanActivateFn = async (route, state) => {
  const router = inject(Router);  
  const keycloak = inject(Keycloak);

  if (!keycloak.authenticated) {
    await keycloak.login({
      redirectUri: window.location.origin + state.url
    });
    return false;
  }
  
  const requiredRoles = route.data?.['roles'] as string[];
  if (!requiredRoles || requiredRoles.length === 0) {
    return true;
  }

  const userRoles = keycloak.realmAccess?.roles || [];
  const hasRole = requiredRoles.every((role) => userRoles.includes(role));

  if (!hasRole) {
    return router.parseUrl('/unauthorized');
  }

  return true;
};