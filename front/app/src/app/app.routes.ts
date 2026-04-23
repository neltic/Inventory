import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

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
        canActivate: [authGuard],
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
        path: 'unauthorized',
        loadComponent: () => import('./shared/components/unauthorized/unauthorized').then(m => m.Unauthorized)
    },
    {
        path: '**',
        redirectTo: 'unauthorized'
    }
];