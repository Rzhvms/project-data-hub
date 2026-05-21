import { Injectable, signal } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

import { IUser } from '../interfaces/user.interface';
import { UserRole } from '../types/user-role.type';

type AccessTokenPayload = {
    email: string;
    // eslint-disable-next-line @typescript-eslint/naming-convention
    given_name: string;
    // eslint-disable-next-line @typescript-eslint/naming-convention
    family_name: string;
    role: UserRole;
}

@Injectable({ providedIn: 'root' })
export class UserService {
    public get user(): IUser | null {
        return this._user();
    }

    private readonly _user = signal<IUser | null>(null);

    public setUserFromToken(token: string): void {
        const payload = jwtDecode<AccessTokenPayload>(token);
        const user: IUser = {
            name: `${payload.given_name} ${payload.family_name}`,
            email: payload.email,
            role: payload.role
        };

        this._user.set(user);
    }

    public removeUser(): void {
        this._user.set(null);
    }
}
