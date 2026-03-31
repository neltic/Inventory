import { Routes } from '@angular/router';

export const BRAND_ROUTES: Routes = [
    {
        path: 'list',
        loadComponent: () => import('./brand-list/brand-list').then(m => m.BrandList),
        title: 'Brand - Setup',
    }
];