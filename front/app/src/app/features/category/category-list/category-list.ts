import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, computed, effect, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChip, MatChipAvatar, MatChipSet, MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { EMPTY_CATEGORY, ICategory } from '@models';
import { CategoryService } from '@services';
import { firstValueFrom } from 'rxjs';
import { BaseComponent } from '../../../shared/components/base/base';
import { CategoryEditDialog } from '../category-edit-dialog/category-edit-dialog';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [
    MatButtonModule, 
    MatIconModule, 
    MatButtonModule,
    MatChip,
    MatChipSet,
    MatChipsModule,
    MatChipAvatar,
    DragDropModule, 
    MatTableModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule
  ],
  templateUrl: './category-list.html',
  styleUrl: './category-list.scss',
})
export class CategoryList extends BaseComponent {
  private categoryService: CategoryService = inject(CategoryService);
  public filterText = signal<string>('');

  filteredCategories = computed(() => {
    const text = this.filterText().toLowerCase();
    const all = this.categoryService.categories();
    if (!text) return all;
    return all.filter(c => c.name.toLowerCase().includes(text));
  });
  
  isDragDisabled = computed(() => this.filterText().length > 0);

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
    const nextOrder = this.categoryService.categories().length > 0 ? Math.max(...this.categoryService.categories().map(c => c.order)) + 1 : 1;
    const category: ICategory = {
      ...EMPTY_CATEGORY,
      order: nextOrder
    };
    await this.openEditDialog(category);
  }

  async openEditDialog(category?: ICategory) {
    const dialogRef = this.dialog.open(CategoryEditDialog, { 
      data: category, 
      panelClass: 'edit-dialog',
      disableClose: true
    });
    await firstValueFrom(dialogRef.afterClosed());    
  }

  async deleteCategory(category: ICategory) {
    const confirmed = await this.openWarning('Are you sure you want to delete this Category (' + category.name + ')?');
    if (!confirmed) {
      return;
    }
    this.categoryService.delete(category.categoryId).subscribe({
      next: () => {   
        this.openSnack('success', 'Category deleted successfully!', 'Ok');
      },
      error: (error) => this.handleError(error)
    });
  }

  async drop(event: CdkDragDrop<ICategory[]>) {

    if (event.previousIndex === event.currentIndex) return;
    
    const originalList = [...this.categoryService.categories()];
    const newList = [...originalList];

    moveItemInArray(newList, event.previousIndex, event.currentIndex);
    
    const optimizedList = newList.map((c, i) => ({ ...c, order: i + 1 }));

    this.categoryService.updateLocalCategories(optimizedList);

    const movedItem = originalList[event.previousIndex];
    const newOrder = event.currentIndex + 1;

    this.categoryService.reorder(movedItem.categoryId, newOrder).subscribe({
      next: () => this.openSnack('success', `Moved to position ${newOrder}`, 'Ok'),
      error: (err) => {
        this.categoryService.updateLocalCategories(originalList);
        this.handleError(err);
      }
    });
  }

  trackById(index: number, item: ICategory) {
    return item.categoryId;
  }

}
