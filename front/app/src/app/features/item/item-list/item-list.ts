import { Component, computed, inject, signal, ViewChild } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDivider } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatList, MatListItem } from '@angular/material/list';
import { MatOption, MatSelect, MatSelectTrigger } from '@angular/material/select';
import { MatDrawer, MatDrawerContainer, MatDrawerContent } from '@angular/material/sidenav';
import { IItem } from '@models';
import { BrandService, CategoryService, ItemService, StorageService } from '@services';
import { BaseComponent } from '../../../shared/components/base/base';
import { TranslateDirective } from "../../../shared/directives/translate-directive";
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { FindInListPipe } from '../../../shared/pipes/find-in-list-pipe';
import { ItemRepeater } from '../item-repeater/item-repeater';

@Component({
    selector: 'app-item',
    providers: [provideNativeDateAdapter()],
    imports: [
        ItemRepeater,
        MatFormFieldModule,
        MatInputModule,
        FormsModule,
        MatButtonModule,
        MatIconModule,
        MatOption,
        MatSelect,
        MatSelectTrigger,
        MatDivider,
        MatList,
        MatListItem,
        MatDrawer,
        MatDrawerContent,
        MatDrawerContainer,
        AsPhotoPipe,
        FindInListPipe,
        TranslateDirective
    ],
    templateUrl: './item-list.html',
    styleUrl: './item-list.scss',
})
export class ItemList extends BaseComponent {
    private itemService: ItemService = inject(ItemService);
    public categoryService: CategoryService = inject(CategoryService);
    public storageService: StorageService = inject(StorageService);
    public brandService: BrandService = inject(BrandService);
    public filterText = signal<string>('');
    public filterCategory = signal<number | null>(null);
    public selectedItem = signal<IItem | null>(null);

    @ViewChild('storageDrawer') drawer!: MatDrawer;

    itemResourceList = rxResource({
        stream: () => {
            return this.itemService.getItems()
        }
    });

    category = computed(() =>
        this.categoryService.getCategoryById(this.selectedItem()?.categoryId ?? 0)
    );

    filteredList = computed(() => {
        const text = this.filterText().toLowerCase();
        const categoryId = this.filterCategory();
        const allItems = this.itemResourceList.value() ?? [];

        return allItems.filter(item => {
            const matchesText = item.name.toLowerCase().includes(text);
            const matchesCategory = categoryId === null || item.categoryId === categoryId;
            return matchesText && matchesCategory;
        });
    });

    onCategoryChange(id: number | null) {
        this.filterCategory.set(id);
    }

    onSearch(event: Event) {
        const val = (event.target as HTMLInputElement).value;
        this.filterText.set(val);
    }

    clearSearch() {
        this.filterText.set('');
    }

    onViewStorage(item: any) {
        this.selectedItem.set(item);
        this.storageService.getStorageByItem(item.itemId).subscribe({
            next: (data) => {
                this.storageService.itemStorage.set(data);
                this.drawer.open();
            },
            error: (err) => console.error(err)
        });
    }

}
