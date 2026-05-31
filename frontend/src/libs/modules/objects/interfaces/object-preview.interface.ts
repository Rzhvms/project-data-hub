import { ObjectStatus } from '../constants/object-status.constant';
import { ObjectType } from '../constants/object-type.constant';

export interface IObjectPreview {
    id: string;
    title: string;
    city: string;
    type: ObjectType;
    status: ObjectStatus;
    projectManager: string;
    createdAt: Date;
    updatedAt: Date;
}
