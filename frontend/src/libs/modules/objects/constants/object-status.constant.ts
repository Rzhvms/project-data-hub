import { IOption } from '@project-data-hub/shared';

export const OBJECT_STATUS_OPTIONS = [
    {
        value: 'draft',
        label: 'Черновик',
    },
    {
        value: 'published',
        label: 'Опубликовано',
    },
    {
        value: 'archived',
        label: 'Архив',
    },
] as const satisfies IOption[];

export type ObjectStatus = (typeof OBJECT_STATUS_OPTIONS)[number]['value'];
