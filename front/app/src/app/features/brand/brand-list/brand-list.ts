import { Component, computed, effect, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { EMPTY_BRAND, IBrand } from '@models';
import { BrandService } from '@services';
import { firstValueFrom } from 'rxjs';
import { BaseComponent } from '../../../shared/components/base/base';
import { BrandEditDialog } from '../brand-edit-dialog/brand-edit-dialog';

@Component({
  selector: 'app-brand-list',
  standalone: true,
  imports: [
    MatButtonModule, 
    MatIconModule, 
    MatChipsModule,
    MatTableModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule
  ],
  templateUrl: './brand-list.html',
  styleUrl: './brand-list.scss',
})
export class BrandList extends BaseComponent {
  private brandService: BrandService = inject(BrandService);
  public filterText = signal<string>('');

  filteredBrands = computed(() => {
    const text = this.filterText().toLowerCase();
    const all = this.brandService.brands();
    if (!text) return all;
    return all.filter(b => b.name.toLowerCase().includes(text));
  });

  constructor() {
    super();    
    effect(() => {
      if (this.params.action() === 'add') {
        this.openAddDialog();        
        this.params.clearQueryAction();
      }
    });
  }

  async openAddDialog() {
    const brand: IBrand = { ...EMPTY_BRAND };
    await this.openEditDialog(brand);
  }

  async openEditDialog(brand: IBrand) {       
    const dialogRef = this.dialog.open(BrandEditDialog, { 
      data: brand, 
      panelClass: 'edit-dialog',
      disableClose: true
    });
    await firstValueFrom(dialogRef.afterClosed());    
  }

  async deleteBrand(brand: IBrand) {
    const confirmed = await this.openWarning(`Are you sure you want to delete the brand "${brand.name}"?`);
    if (!confirmed) return;

    this.brandService.delete(brand.brandId).subscribe({
      next: () => this.openSnack('success', 'Brand deleted successfully!', 'Ok'),
      error: (error) => this.handleError(error)
    });
  }

  trackById(index: number, item: IBrand) {
    return item.brandId;
  }
}