import { Component, computed, inject, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from "@angular/material/icon";
import { Router } from '@angular/router';
import { IItem, Role } from '@models';
import { CategoryService } from '@services';
import { HasRoleDirective } from '../../../shared/directives/has-role-directive';
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';
import { TranslateDirective } from "../../../shared/directives/translate-directive";
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';

@Component({
  selector: 'app-item-repeater',
  imports: [MatIcon, MatCardModule, MatButtonModule, ImgFallbackDirective, AsPhotoPipe, TranslateDirective, HasRoleDirective],
  templateUrl: './item-repeater.html',
  styleUrl: './item-repeater.scss',
})
export class ItemRepeater {
  private router: Router = inject(Router);
  private categoryService: CategoryService = inject(CategoryService);  
  public item = input.required<IItem>();
  public viewStorageEvent = output<any>();
  public readonly Role = Role;

  category = computed(() => 
    this.categoryService.getCategoryById(this.item().categoryId)
  );
  
  goToEdit() {
    this.router.navigate(['/item/edit', this.item().itemId]);
  }

  goToDetails() {
    this.router.navigate(['/item/detail', this.item().itemId]);
  }

  viewStorage() {
    this.viewStorageEvent.emit(this.item());
  }
  
}
