import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { IBox } from '@models';
import { BoxService, BrandService, CategoryService } from '@services';
import { BaseComponent } from '../../../shared/components/base/base';
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { RelativeTimePipe } from '../../../shared/pipes/relative-time-pipe';
import { BoxBreadcrumb } from '../box-breadcrumb/box-breadcrumb';

@Component({
  selector: 'app-box-detail',
  standalone: true,
  imports: [
    MatIcon, 
    MatCardModule, 
    MatButtonModule, 
    RelativeTimePipe, 
    DatePipe, 
    MatTooltipModule, 
    MatProgressSpinnerModule, 
    ImgFallbackDirective,
    BoxBreadcrumb,
    AsPhotoPipe
  ],
  templateUrl: './box-detail.html',
  styleUrl: './box-detail.scss',
})
export class BoxDetail extends BaseComponent {  
  private boxService: BoxService = inject(BoxService);
  protected categoryService: CategoryService = inject(CategoryService);
  protected brandService: BrandService = inject(BrandService);
  public isDeleting = signal(false);

  boxResource = rxResource<IBox, any>({
    stream: () => { 
      return this.boxService.getBoxBy(this.params.boxId()); 
    }
  });

  async deleteBox(boxId: number): Promise<void> {  

    const confirmed = await this.openWarning('Are you sure you want to delete this box?');
    if (!confirmed) {
      return;
    }

    this.isDeleting.set(true);
    this.boxService.deleteBox(boxId).subscribe({
      next: () => {   
        const snackRef = this.openSnack('success', '¡Box deleted successfully!', 'Ok');
        snackRef.afterDismissed().subscribe(() => {
          this.goBack();
        });
      },
      error: (error) => {
        this.handleError(error)
        this.isDeleting.set(false);
      }      
    });
  }
}
