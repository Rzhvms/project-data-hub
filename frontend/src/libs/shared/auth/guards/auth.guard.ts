import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AppRoute, LocalStorageKeys } from '../../enums';

export const authGuard: CanActivateFn = () => {
    const router: Router = inject(Router);
    const accessToken = localStorage.getItem(LocalStorageKeys.AccessToken);

    return accessToken
        ? true
        : router.createUrlTree([AppRoute.LoginPage])
};
