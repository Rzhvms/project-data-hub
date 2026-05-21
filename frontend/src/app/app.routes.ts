import { Routes } from '@angular/router';
import { authGuard, guestGuard } from '@project-data-hub/modules/auth';
import { AppRoute } from '@project-data-hub/shared';

export const routes: Routes = [
    {
        path: AppRoute.LoginPage,
        loadComponent: () => import('./pages/login/login.page').then(m => m.LoginPageComponent),
        canActivate: [guestGuard]
    },
    {
        path: AppRoute.MainPage,
        loadComponent: () => import('./pages/main/main.page').then(m => m.MainPageComponent),
        loadChildren: () => import('./pages/main/main-page.routes').then(m => m.MAIN_PAGE_ROUTES),
        canActivate: [authGuard],
        canActivateChild: [authGuard]
    }
];
