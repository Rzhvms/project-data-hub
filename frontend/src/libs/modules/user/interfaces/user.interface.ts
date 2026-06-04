import { UserRole } from '../types/user-role.type';

export interface IUser {
    id: string;
    email: string;
    name: string;
    role: UserRole;
}
