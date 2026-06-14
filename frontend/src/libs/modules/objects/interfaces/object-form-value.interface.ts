import { IObject } from './object.interface';

export type IObjectFormValue = Omit<IObject, 'id' | 'createdAt' | 'updatedAt' | 'status'>;
