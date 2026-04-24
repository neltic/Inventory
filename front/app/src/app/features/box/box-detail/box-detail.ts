import { DatePipe } from '@angular/common';
import { Component, effect, inject, Injector, signal } from '@angular/core';
import { rxResource, toObservable } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { IBox, IBoxFullPath } from '@models';
import { BoxService, BrandService, CategoryService } from '@services';
import { filter, firstValueFrom, switchMap } from 'rxjs';
import { BaseComponent } from '../../../shared/components/base/base';
import { HasRoleDirective } from "../../../shared/directives/has-role-directive";
import { ImgFallbackDirective } from '../../../shared/directives/img-fallback';
import { TranslateDirective } from '../../../shared/directives/translate-directive';
import { AsPhotoPipe } from '../../../shared/pipes/as-photo-pipe';
import { RelativeTimePipe } from '../../../shared/pipes/relative-time-pipe';
import { BoxBreadcrumb } from '../box-breadcrumb/box-breadcrumb';
import { BoxParentSelectDialog } from '../box-parent-select-dialog/box-parent-select-dialog';

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
    AsPhotoPipe,
    TranslateDirective,
    HasRoleDirective
],
  templateUrl: './box-detail.html',
  styleUrl: './box-detail.scss',
})
export class BoxDetail extends BaseComponent {  
  private boxService: BoxService = inject(BoxService);
  protected categoryService: CategoryService = inject(CategoryService);
  protected brandService: BrandService = inject(BrandService);
  public isDeleting = signal(false);
  public currentPath = signal<IBoxFullPath[] | string | null | undefined>(undefined);
  private injector: Injector = inject(Injector);
  
  constructor() {
    super();
    effect(() => {
      const box = this.boxResource.value();
      if (box) {
        this.currentPath.set(box.fullPath);
      }
    });
  }

  boxResource = rxResource<IBox, any>({
    stream: () => { 
        return toObservable(this.params.boxId, { injector: this.injector }).pipe(        
        filter(id => id > 0),
        switchMap(id => this.boxService.getBoxBy(id))
      );      
    }
  });

  async openBoxParentSelectDialog(box: IBox) {
    const dialogRef = this.dialog.open(BoxParentSelectDialog, { 
          data: box, 
          panelClass: 'edit-dialog-xx',
          disableClose: true
        });
    const isMoved = await firstValueFrom(dialogRef.afterClosed()); 
    if (isMoved) {      
      this.boxService.getBoxFullPath(box.boxId).subscribe({
        next: (newPath) => {          
          this.currentPath.set(newPath);          
        }
      });
    }
  }

  async deleteBox(boxId: number): Promise<void> {  

    const confirmed = await this.openWarning('Message.CONFIRM_DELETE_BOX');
    if (!confirmed) {
      return;
    }

    this.isDeleting.set(true);
    this.boxService.deleteBox(boxId).subscribe({
      next: () => {   
        const snackRef = this.openSnack('success', 'Global.OK', 'Message.BOX_DELETED');
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
