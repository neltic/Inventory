import { Routes } from '@angular/router';

export const routes: Routes = [
    { 
        path: '', 
        redirectTo: 'welcome',
        pathMatch: 'full' 
    },
    {
        path: 'welcome',
        loadComponent: () => import('./features/welcome/welcome').then(m => m.Welcome),
        title: 'Home',
    },
    {
        path: 'storage',
        loadComponent: () => import('./features/storage/storage').then(m => m.Storage),
        title: 'Storage',
    },
    {
        path: 'box',
        loadChildren: () => import('./features/box/box.routes').then(m => m.BOX_ROUTES)
    },
    {
        path: 'item',
        loadChildren: () => import('./features/item/item.routes').then(m => m.ITEM_ROUTES)
    },
    {
        path: 'category',
        loadChildren: () => import('./features/category/category.routes').then(m => m.CATEGORY_ROUTES)
    },
    {
        path: 'brand',
        loadChildren: () => import('./features/brand/brand.routes').then(m => m.BRAND_ROUTES)
    },
    {
        path: '**',
        redirectTo: ''
    }
];