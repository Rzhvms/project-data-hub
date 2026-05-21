import { UserRole } from '../types/user-role.type';

export interface IUser {
    email: string;
    name: string;
    role: UserRole
}
