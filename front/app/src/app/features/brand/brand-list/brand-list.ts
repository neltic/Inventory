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
import { HasRoleDirective } from '../../../shared/directives/has-role-directive';
import { TranslateDirective } from '../../../shared/directives/translate-directive';
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
        FormsModule,
        TranslateDirective,
        HasRoleDirective
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

    readonly displayedColumns = computed(() => {
        const isEditor = this.securityService.hasRole(this.Role.Editor);
        return [
            'id',
            'brand',
            'brand-alt',
            'name',
            'scopes',
            ...(isEditor ? ['actions'] : [])
        ];
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
        const confirmed = await this.openWarning('Message.CONFIRM_DELETE_BRAND', [brand.name]);
        if (!confirmed) return;

        this.brandService.delete(brand.brandId).subscribe({
            next: () => this.openSnack('success', 'Global.OK', 'Message.BRAND_DELETED'),
            error: (error) => this.handleError(error)
        });
    }

    trackById(index: number, item: IBrand) {
        return item.brandId;
    }
}