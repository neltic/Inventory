import { Component, inject, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { BoxService } from '@services';

@Component({
  selector: 'app-box-breadcrumb',
  imports: [RouterLink, MatButtonModule, MatIconModule],
  templateUrl: './box-breadcrumb.html',
  styleUrl: './box-breadcrumb.scss',
})
export class BoxBreadcrumb {
  public boxService: BoxService = inject(BoxService);
  public fullPath = input.required<string | null | undefined>();
}
