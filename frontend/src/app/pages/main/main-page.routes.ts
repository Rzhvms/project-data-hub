import { Routes } from '@angular/router';
import { AppRoute } from '@project-data-hub/shared';

export const MAIN_PAGE_ROUTES: Routes = [
    {
        path: AppRoute.ObjectsPage,
        loadComponent: () => import('./children/objects/objects.page').then(m => m.ObjectsPageComponent)
    },
    {
        path: AppRoute.MediaPage,
        loadComponent: () => import('./children/media/media.page').then(m => m.MediaPageComponent)
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
