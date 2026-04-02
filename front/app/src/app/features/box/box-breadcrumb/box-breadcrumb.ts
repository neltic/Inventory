import { Component, inject, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { IBoxFullPath } from '@models';
import { BoxService } from '@services';
import { BaseComponent } from '../../../shared/components/base/base';

@Component({
  selector: 'app-box-breadcrumb',
  imports: [RouterLink, MatButtonModule, MatIconModule],
  templateUrl: './box-breadcrumb.html',
  styleUrl: './box-breadcrumb.scss',
})
export class BoxBreadcrumb extends BaseComponent {
  public boxService: BoxService = inject(BoxService);
  public fullPath = input.required<IBoxFullPath[] | string | null | undefined>();
}
