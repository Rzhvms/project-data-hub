import { IOption } from '../../../shared/interfaces';

export const OBJECT_STATUS_OPTIONS = [
    {
        value: 'draft',
        label: 'Черновик'
    },
    {
        value: 'published',
        label: 'Опубликовано'
    },
    {
        value: 'archived',
        label: 'Архив'
    }
] as const satisfies readonly IOption[];

export type ObjectStatus = (typeof OBJECT_STATUS_OPTIONS)[number]['value'];
