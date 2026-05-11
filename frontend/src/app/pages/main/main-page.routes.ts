import { Routes } from '@angular/router';

import { AppRoute } from '../../../libs/shared/enums';

export const MAIN_PAGE_ROUTES: Routes = [
    {
        path: AppRoute.ObjectsPage,
        loadComponent: () => import('./children/objects/objects.page').then(m => m.ObjectsPageComponent)
    },
    {
        path: AppRoute.TemplatesPage,
        loadComponent: () => import('./children/templates/templates.page').then(m => m.TemplatesPageComponent)
    },
    {
        path: AppRoute.MediaPage,
        loadComponent: () => import('./children/media/media.page').then(m => m.MediaPageComponent)
    },
    {
        path: AppRoute.FoldersPage,
        loadComponent: () => import('./children/folders/folders.page').then(m => m.FoldersPageComponent)
    },
    {
        path: AppRoute.WPSettingsPage,
        loadComponent: () => import('./children/wp-settings/wp-settings.page').then(m => m.WPSettingsPageComponent)
    },
    {
        path: '',
        redirectTo: AppRoute.ObjectsPage,
        pathMatch: 'full'
    }
];
