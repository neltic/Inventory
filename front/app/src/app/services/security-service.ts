import { inject, Injectable, signal } from '@angular/core';
import { Role } from '@models';
import Keycloak from 'keycloak-js';

@Injectable({
  providedIn: 'root',
})
export class SecurityService {
  private keycloak = inject(Keycloak); 
  private _givenName = signal<string>(this.keycloak.idTokenParsed?.['given_name'] ?? '');
  public givenName = this._givenName.asReadonly();

  logout() {
    this.keycloak.logout({ redirectUri: window.location.origin });
  }

  logoutAndRelogin() {
    this.keycloak.logout({      
      redirectUri: window.location.origin + '/login-required' 
    });
  }

  hasRole(role: Role) : boolean {
    return this.keycloak.hasRealmRole(role);
  }
  
}
