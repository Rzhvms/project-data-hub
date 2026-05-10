import { HttpErrorResponse, HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, filter, Observable, switchMap, take, throwError } from 'rxjs';

import { ApiRoute, AppRoute } from '../../enums';
import { AuthService } from '../services/auth.service';

let isRefreshing = false;
const refreshSubject$ = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
    const authService: AuthService = inject(AuthService);
    const router: Router = inject(Router);

    return next(
        addAuthorizationHeader(req, authService.accessToken)
    ).pipe(
        catchError((error: HttpErrorResponse) => {
            if (shouldSkip401Handling(req, error)) {
                return throwError(() => error)
            }

            if (!isRefreshing) {
                return refreshTokens(
                    req,
                    next,
                    authService,
                    router
                );
            }

            return waitForRefreshAndRepeat(req, next);
        })
    );
};

function addAuthorizationHeader(
    req: HttpRequest<unknown>,
    token: string | null
): HttpRequest<unknown> {
    if (!token) {
        return req;
    }

    return req.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`
        }
    });
}

function shouldSkip401Handling(
    req: HttpRequest<unknown>,
    error: HttpErrorResponse
): boolean {
    return (
        error.status !== 401 ||
        req.url.includes(ApiRoute.Login) ||
        req.url.includes(ApiRoute.RefreshTokens)
    );
}

function refreshTokens(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn,
    authService: AuthService,
    router: Router
): Observable<HttpEvent<unknown>> {
    isRefreshing = true;
    refreshSubject$.next(null);

    return authService.refresh().pipe(
        switchMap((tokens) => {
            isRefreshing = false;
            refreshSubject$.next(tokens.accessToken);

            return next(
                addAuthorizationHeader(req, tokens.accessToken)
            );
        }),
        catchError((error) => {
            isRefreshing = false;
            authService.unauthorize();
            void router.navigate([AppRoute.LoginPage]);

            return throwError(() => error);
        })
    );
}

function waitForRefreshAndRepeat(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
    return refreshSubject$.pipe(
        filter((token): token is string => token !== null),
        take(1),
        switchMap((token) => next(
            addAuthorizationHeader(req, token)
        ))
    );
}
