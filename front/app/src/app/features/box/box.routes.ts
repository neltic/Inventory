import { Routes } from '@angular/router';

export const BOX_ROUTES: Routes = [
    {
        path: 'list/:parentBoxId',
        loadComponent: () => import('./box-list/box-list').then(m => m.BoxList),
        title: 'Box - Search',
    },
    {
        path: 'detail/:boxId',
        loadComponent: () => import('./box-detail/box-detail').then(m => m.BoxDetail),
        title: 'Box - Details',
    },
    {
        path: 'edit/:boxId',
        loadComponent: () => import('./box-edit/box-edit').then(m => m.BoxEdit),
        title: 'Box - Edit',
    },
    {
        path: 'new/:parentBoxId',
        loadComponent: () => import('./box-new/box-new').then(m => m.BoxNew),
        title: 'Box - New',
    }
];