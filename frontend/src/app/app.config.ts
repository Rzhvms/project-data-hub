import { ApplicationConfig, provideBrowserGlobalErrorListeners, signal } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideTaiga, TUI_VALIDATION_ERRORS } from '@taiga-ui/core';
import {TUI_LANGUAGE, TUI_RUSSIAN_LANGUAGE} from '@taiga-ui/i18n';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from '../libs/shared/auth';

export const appConfig: ApplicationConfig = {
    providers: [
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
