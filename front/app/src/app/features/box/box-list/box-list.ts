import { Component, computed, effect, inject, signal, ViewChild } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatList, MatListItem } from '@angular/material/list';
import { MatDrawer, MatDrawerContainer, MatDrawerContent } from '@angular/material/sidenav';
import { IBox } from '@models';
import { BoxService, BrandService, CategoryService, StorageService } from '@services';
import { BaseComponent } from '../../../shared/components/base/base';
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';
import { TranslateDirective } from '../../../shared/directives/translate-directive';
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { FindInListPipe } from '../../../shared/pipes/find-in-list-pipe';
import { BoxRepeater } from '../box-repeater/box-repeater';

@Component({
  selector: 'app-box',
  imports: [
    BoxRepeater,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatDivider,
    MatList,
    MatListItem,
    MatDrawer,
    MatDrawerContent,
    MatDrawerContainer,
    MatBadgeModule,
    FindInListPipe, 
    AsPhotoPipe,
    ImgFallbackDirective,
    TranslateDirective
],
  templateUrl: './box-list.html',
  styleUrl: './box-list.scss',
})
export class BoxList extends BaseComponent {  
  private boxService: BoxService = inject(BoxService);
  public storageService: StorageService = inject(StorageService);
  public categoryService: CategoryService = inject(CategoryService);
  public brandService: BrandService = inject(BrandService);
  public filterText = signal<string>('');
  public selectedBoxName = signal<string>('');

  @ViewChild('itemDrawer') drawer!: MatDrawer;

  constructor() {
    super();
    effect(() => {
      this.params.parentBoxId();
      this.boxResourceList.reload();
    });
  }  

  boxResourceList = rxResource<IBox[], any>({    
    stream: () => this.boxService.getBoxesBy(this.params.parentBoxId())
  });

  filteredList = computed(() => {
    const rawData = this.boxResourceList.value() ?? [];
    const query = this.filterText().toLowerCase();
    if (!query) return rawData;
    return rawData.filter(item => 
      item.name.toLowerCase().includes(query)
    );
  });

  onSearch(event: Event) {
    const val = (event.target as HTMLInputElement).value;
    this.filterText.set(val);
  }

  clearSearch() {
    this.filterText.set('');
  }

  onShowItems(box: any) {
    this.selectedBoxName.set(box.name);

    this.storageService.getItemsByBox(box.boxId).subscribe({
      next: (data) => {
        this.storageService.itemsInBox.set(data);
        this.drawer.open();
      },
      error: (error) => this.handleError(error)
    });
  }

}
