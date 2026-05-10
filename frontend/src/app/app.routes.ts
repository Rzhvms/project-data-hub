import { Routes } from '@angular/router';

import { AppRoute } from '../libs/shared/enums';
import { authGuard, guestGuard } from '../libs/shared/auth';

export const routes: Routes = [
    {
        path: AppRoute.LoginPage,
        loadComponent: () => import('./pages/login/login.page').then(m => m.LoginPageComponent),
        canActivate: [guestGuard]
    },
    {
        path: AppRoute.MainPage,
        loadComponent: () => import('./pages/main/main.page').then(m => m.MainPageComponent),
        canActivate: [authGuard]
    }
];
