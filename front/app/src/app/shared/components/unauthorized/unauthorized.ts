import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { SecurityService } from '@services';
import { TranslateDirective } from '../../directives/translate-directive';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule, TranslateDirective],
  templateUrl: './unauthorized.html',
  styleUrl: './unauthorized.scss',
})
export class Unauthorized {
  private router = inject(Router);  
  private securityService: SecurityService = inject(SecurityService);

  goHome() {
    this.router.navigate(['/']);
  }

  logout() {
    this.securityService.logoutAndRelogin(); 
  }
}
