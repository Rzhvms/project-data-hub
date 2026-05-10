import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { ApiRoute } from '../../enums';
import { LoginDto } from '../dto/login.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly _http: HttpClient = inject(HttpClient);

    public authorize(credits: LoginDto): Observable<void> {
        return this._http.post(ApiRoute.Login, credits)
            .pipe(
                tap((item) => console.log(item))
            )
    }
}
