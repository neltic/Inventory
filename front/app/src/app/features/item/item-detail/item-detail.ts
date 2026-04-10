import { DatePipe } from '@angular/common';
import { Component, computed, effect, inject, signal } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterLink } from '@angular/router';
import { IItem, IItemLocation } from '@models';
import { BrandService, CategoryService, ItemService, StorageService } from '@services';
import { BaseComponent } from '../../../shared/components/base/base';
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { RelativeTimePipe } from '../../../shared/pipes/relative-time-pipe';

@Component({
  selector: 'app-item-detail',
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
    RouterLink,
    AsPhotoPipe
  ],
  templateUrl: './item-detail.html',
  styleUrl: './item-detail.scss',
})
export class ItemDetail extends BaseComponent {  
  private storageService: StorageService = inject(StorageService);
  public itemService: ItemService = inject(ItemService);
  public categoryService: CategoryService = inject(CategoryService);
  public brandService: BrandService = inject(BrandService);
  public isDeleting = signal<boolean>(false);
  public itemLocations = signal<IItemLocation[]>([]);
  public isStored = computed(() => this.itemLocations().length > 0);

  itemResource = rxResource<IItem, any>({
    stream: () => { 
      return this.itemService.getItemBy(this.params.itemId()); 
    }
  });

  constructor() {
    super();
    effect(() => {
      const item = this.itemResource.value();
      if (item) {  
        this.itemService.getItemLocations(item.itemId).subscribe({
          next: (locations) => this.itemLocations.set(locations),
          error: (err) => this.handleError(err)
        });
      }
    });
  }

  async deleteItem(itemId: number): Promise<void> {
  
    const confirmed = await this.openWarning('Are you sure you want to delete this Item?');
    if (!confirmed) {
      return;
    }

    this.isDeleting.set(true);
    this.itemService.deleteItem(itemId).subscribe({
      next: () => {   
        const snackRef = this.openSnack('success', 'Item deleted successfully!', 'Ok');
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

  async removeFromBox(location: IItemLocation, itemId: number, brandName: string) {

    const confirmed = await this.openWarning('Are you sure you want to remove this "'+ brandName +'" branded item from the box "' + location.name + '"?');
    if (!confirmed) {
      return;
    }

    this.isDeleting.set(true);
    this.storageService.remove(location.boxId, itemId, location.brandId).subscribe({
      next: () => { 
        this.itemService.getItemLocations(itemId).subscribe({
          next: (locations) => {
            this.itemLocations.set(locations);
            this.isDeleting.set(false); 
            this.openSnack('success', 'Item removed from the box successfully', 'Ok');
          },
          error: (error) => {
            this.handleError(error);
            this.isDeleting.set(false);
          }
        });        
      },
      error: (error) => {
        this.handleError(error, 'An error occurred while trying to remove item relashionsip')
        this.isDeleting.set(false);
      }
    });
  }
  
}
