import { Routes } from '@angular/router';
import { authGuard } from '@project-data-hub/modules/auth';

export const OBJECTS_ROUTES: Routes = [
    {
        path: 'edit/:objectId',
        loadComponent: () => import('./children/object-edit/object-edit.page').then(m => m.ObjectEditPageComponent),
        canActivate: [authGuard]
    },
    {
        path: 'view/:objectId',
        loadComponent: () => import('./children/object-view/object-view.page').then(m => m.ObjectViewPageComponent),
        canActivate: [authGuard]
    },
    {
        path: 'create',
        loadComponent: () => import('./children/object-create/object-create.page').then(m => m.ObjectCreatePageComponent),
        canActivate: [authGuard]
    },
    {
        path: '',
        loadComponent: () => import('./objects.page').then(m => m.ObjectsPageComponent),
        canActivate: [authGuard],
    },
];
