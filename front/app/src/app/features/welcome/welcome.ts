import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatRippleModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { TranslateDirective } from "../../shared/directives/translate-directive";

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatRippleModule,    
    TranslateDirective
],  
  templateUrl: './welcome.html',
  styleUrl: './welcome.scss',
})
export class Welcome {

}
