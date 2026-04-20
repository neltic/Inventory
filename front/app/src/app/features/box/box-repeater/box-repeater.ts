import { Component, computed, inject, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from "@angular/material/icon";
import { Router } from '@angular/router';
import { IBox } from '@models';
import { BrandService, CategoryService } from '@services';
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';
import { TranslateDirective } from "../../../shared/directives/translate-directive";
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';

@Component({
  selector: 'app-box-repeater',
  imports: [MatIcon, MatCardModule, MatButtonModule, ImgFallbackDirective, AsPhotoPipe, TranslateDirective],
  templateUrl: './box-repeater.html',
  styleUrl: './box-repeater.scss',
})
export class BoxRepeater {
  private router: Router = inject(Router);
  private brandService: BrandService = inject(BrandService);
  private categoryService: CategoryService = inject(CategoryService);
  public box = input.required<IBox>();
  public showItemsEvent = output<any>();
  
  category = computed(() => 
    this.categoryService.getCategoryById(this.box().categoryId)
  );

  brand = computed(() => 
    this.brandService.getBrandById(this.box().brandId)
  );

  goToEdit() {
    this.router.navigate(['/box/edit', this.box().boxId]);
  }

  goToDetails() {
    this.router.navigate(['/box/detail', this.box().boxId]);
  }

  goToShowContent() {
    this.router.navigate(['/box/list', this.box().boxId]);
  }
  
  showItems() {
    this.showItemsEvent.emit(this.box());
  }
  
}
