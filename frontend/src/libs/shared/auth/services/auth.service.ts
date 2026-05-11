import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { ApiRoute, LocalStorageKeys } from '../../enums';
import { UserService } from '../../user';
import { getFullApiRoute } from '../../utils';
import { LoginDto } from '../dto/login.dto';
import { LoginRdo } from '../rdo/login.rdo';

@Injectable({ providedIn: 'root' })
export class AuthService {
    public get accessToken(): string | null {
        return localStorage.getItem(LocalStorageKeys.AccessToken);
    }

    private readonly _http: HttpClient = inject(HttpClient);
    private readonly _userService: UserService = inject(UserService);

    public authorize(credits: LoginDto): Observable<LoginRdo> {
        return this._http.post<LoginRdo>(getFullApiRoute(ApiRoute.Login), credits)
            .pipe(
                tap((tokens) => {
                    this.saveTokens(tokens);
                    this._userService.setUserFromToken(tokens.accessToken);
                })
            )
    }

    public refresh(): Observable<LoginRdo> {
        const tokens = {
            accessToken: localStorage.getItem(LocalStorageKeys.AccessToken),
            refreshToken: localStorage.getItem(LocalStorageKeys.RefreshToken)
        };

        return this._http.post<LoginRdo>(
            getFullApiRoute(ApiRoute.RefreshTokens),
            tokens
        ).pipe(
            tap((newTokens) => this.saveTokens(newTokens))
        )
    }

    public unauthorize(): Observable<void> {
        return this._http.post<void>(
            getFullApiRoute(ApiRoute.RevokeTokens),
            {}
        ).pipe(
            tap(() => {
                this.removeTokens();
                this._userService.removeUser();
            })
        )
    }

    private saveTokens(tokens: LoginRdo): void {
        localStorage.setItem(LocalStorageKeys.AccessToken, tokens.accessToken);
        localStorage.setItem(LocalStorageKeys.RefreshToken, tokens.refreshToken);
    }

    private removeTokens(): void {
        localStorage.removeItem(LocalStorageKeys.AccessToken);
        localStorage.removeItem(LocalStorageKeys.RefreshToken);
    }
}
