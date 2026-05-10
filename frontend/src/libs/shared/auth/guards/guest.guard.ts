import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AppRoute, LocalStorageKeys } from '../../enums';

export const guestGuard: CanActivateFn = () => {
    const accessToken = localStorage.getItem(LocalStorageKeys.AccessToken);
    const router: Router = inject(Router);

    return accessToken
        ? router.createUrlTree([AppRoute.MainPage])
        : true;
}
