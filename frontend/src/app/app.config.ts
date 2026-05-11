import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, signal } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideTaiga, TUI_VALIDATION_ERRORS } from '@taiga-ui/core';
import {TUI_LANGUAGE, TUI_RUSSIAN_LANGUAGE} from '@taiga-ui/i18n';

import { authInterceptor } from '../libs/shared/auth';
import { LocalStorageKeys } from '../libs/shared/enums';
import { UserService } from '../libs/shared/user';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
    providers: [
        provideAppInitializer(() => {
            const userService: UserService = inject(UserService);
            const token = localStorage.getItem(LocalStorageKeys.AccessToken);

            if (token) {
                userService.setUserFromToken(token);
            }
        }),
        provideBrowserGlobalErrorListeners(),
        provideRouter(routes),
        provideHttpClient(
            withInterceptors([
                authInterceptor
            ])
        ),
        provideTaiga(),
        {
            provide: TUI_LANGUAGE,
            useValue: signal(TUI_RUSSIAN_LANGUAGE)
        },
        {
            provide: TUI_VALIDATION_ERRORS,
            useFactory: () => ({
                required: 'Поле обязательно для заполнения',
                email: 'Введите валидный email адрес'
            })
        }
    ],
};
