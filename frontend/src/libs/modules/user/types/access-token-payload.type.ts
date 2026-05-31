import { UserRole } from './user-role.type';

export type AccessTokenPayload = {
    nameid: string;
    email: string;
    // eslint-disable-next-line @typescript-eslint/naming-convention
    given_name: string;
    // eslint-disable-next-line @typescript-eslint/naming-convention
    family_name: string;
    role: UserRole;
};
