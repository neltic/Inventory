import { Routes } from '@angular/router';

export const CATEGORY_ROUTES: Routes = [
    {
        path: 'list',
        loadComponent: () => import('./category-list/category-list').then(m => m.CategoryList),
        title: 'Category - List',
    }
];