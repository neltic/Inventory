import { Routes } from '@angular/router';
import { Role } from '@models';

export const ITEM_ROUTES: Routes = [
    {
        path: 'list',
        loadComponent: () => import('./item-list/item-list').then(m => m.ItemList),
        title: 'Item - List',
    },
    {
        path: 'detail/:itemId',
        loadComponent: () => import('./item-detail/item-detail').then(m => m.ItemDetail),
        title: 'Item - Details',
    },
    {
        path: 'edit/:itemId',
        loadComponent: () => import('./item-edit/item-edit').then(m => m.ItemEdit),
        title: 'Item - Edit',
        data: { roles: [Role.Operator] }
    },
    {
        path: 'new',
        loadComponent: () => import('./item-new/item-new').then(m => m.ItemNew),
        title: 'Item - Edit',
        data: { roles: [Role.Operator] }
    }
];