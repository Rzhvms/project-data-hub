import { Routes } from '@angular/router';

import { AppRoute } from '../libs/shared/enums';

export const routes: Routes = [
    {
        path: AppRoute.LoginPage,
        loadComponent: () => import('./pages/login/login.page').then(m => m.LoginPageComponent)
    }
];
