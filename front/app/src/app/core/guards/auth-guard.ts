import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { Role } from '@models';
import Keycloak from 'keycloak-js';

export const authGuard: CanActivateChildFn = async (route, state) => {
    const router = inject(Router);
    const keycloak = inject(Keycloak);

    if (!keycloak.authenticated) {
        await keycloak.login({
            redirectUri: window.location.origin + state.url
        });
        return false;
    }

    const requiredRoles = route.data?.['roles'] as Role[];

    if (!requiredRoles || requiredRoles.length === 0) return true;

    const userRoles = keycloak.realmAccess?.roles || [];
    const hasRole = requiredRoles.some((role) => userRoles.includes(role));

    if (hasRole) return true;

    return router.parseUrl('/unauthorized');
};