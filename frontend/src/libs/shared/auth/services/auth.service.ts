import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { ApiRoute, LocalStorageKeys } from '../../enums';
import { getFullApiRoute } from '../../utils';
import { LoginDto } from '../dto/login.dto';
import { LoginRdo } from '../rdo/login.rdo';

@Injectable({ providedIn: 'root' })
export class AuthService {
    public get accessToken(): string | null {
        return localStorage.getItem(LocalStorageKeys.AccessToken);
    }

    private readonly _http: HttpClient = inject(HttpClient);

    public authorize(credits: LoginDto): Observable<LoginRdo> {
        return this._http.post<LoginRdo>(getFullApiRoute(ApiRoute.Login), credits)
            .pipe(
                tap((tokens) => this.saveTokens(tokens))
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

    public unauthorize(): void {
        localStorage.removeItem(LocalStorageKeys.AccessToken);
        localStorage.removeItem(LocalStorageKeys.RefreshToken);
    }

    private saveTokens(tokens: LoginRdo): void {
        localStorage.setItem(LocalStorageKeys.AccessToken, tokens.accessToken);
        localStorage.setItem(LocalStorageKeys.RefreshToken, tokens.refreshToken);
    }
}
