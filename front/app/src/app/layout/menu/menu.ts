import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { RouterLink } from '@angular/router';
import { BaseComponent } from '../../shared/components/base/base';

@Component({
  selector: 'app-menu',
  imports: [RouterLink, MatIconModule, MatButtonModule, MatMenuModule],
  templateUrl: './menu.html',
  styleUrl: './menu.scss',
})
export class Menu extends BaseComponent {
  
}
