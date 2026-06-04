import { Injectable, signal } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

import { IUser } from '../interfaces/user.interface';
import { AccessTokenPayload } from '../types/access-token-payload.type';

@Injectable({ providedIn: 'root' })
export class UserService {
    public get user(): IUser | null {
        return this._user();
    }

    private readonly _user = signal<IUser | null>(null);

    public setUserFromToken(token: string): void {
        const payload = jwtDecode<AccessTokenPayload>(token);
        const user: IUser = {
            id: payload.nameid,
            name: `${payload.given_name} ${payload.family_name}`,
            email: payload.email,
            role: payload.role,
        };

        this._user.set(user);
    }

    public removeUser(): void {
        this._user.set(null);
    }
}
