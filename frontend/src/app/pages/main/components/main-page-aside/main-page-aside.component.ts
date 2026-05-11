import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { TuiButton, TuiDropdown, TuiHint, TuiIcon } from '@taiga-ui/core';
import { TuiBadge, TuiButtonLoading } from '@taiga-ui/kit';
import { finalize, take, tap } from 'rxjs';

import { AuthService } from '../../../../../libs/shared/auth';
import { AppRoute } from '../../../../../libs/shared/enums';
import { ICON_RESOLVER_PROVIDER } from '../../../../../libs/shared/providers';
import { IUser, UserService } from '../../../../../libs/shared/user';
import { MainPageAsideItem } from './types/main-page-aside-item.type';

@Component({
    selector: 'main-page-aside',
    templateUrl: './main-page-aside.component.html',
    styleUrl: './styles/main-page-aside.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiButtonLoading,
        TuiHint,
        RouterLink,
        RouterLinkActive,
        TuiIcon,
        TuiDropdown,
        TuiBadge
    ],
    providers: [ICON_RESOLVER_PROVIDER],
})
export class MainPageAsideComponent {
    protected readonly user: IUser | null = null;
    protected readonly isLogoutButtonLoading = signal<boolean>(false);
    protected readonly buttonList: MainPageAsideItem[] = [
        { icon: '@tui.boxes', hint: 'Объекты', routerLink: AppRoute.ObjectsPage },
        { icon: '@tui.layout-template', hint: 'Шаблоны', routerLink: AppRoute.TemplatesPage },
        { icon: '@tui.image', hint: 'Медиа', routerLink: AppRoute.MediaPage },
        { icon: '@tui.folder', hint: 'Папки', routerLink: AppRoute.FoldersPage },
        { icon: 'wordpress-logo', hint: 'Конфигурация WordPress', routerLink: AppRoute.WPSettingsPage }
    ];

    private readonly _authService: AuthService = inject(AuthService);
    private readonly _userService: UserService = inject(UserService);
    private readonly _router: Router = inject(Router);

    constructor() {
        this.user = this._userService.user
    }

    protected logout(): void {
        this.isLogoutButtonLoading.set(true);
        this._authService.unauthorize()
            .pipe(
                tap(() => this._router.navigate([AppRoute.LoginPage])),
                take(1),
                finalize(() => this.isLogoutButtonLoading.set(false))
            )
            .subscribe();
    }
}
