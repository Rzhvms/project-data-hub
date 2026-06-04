import { IOption } from '@project-data-hub/shared';

export const OBJECT_STATUS_OPTIONS = [
    {
        value: 'published',
        label: 'Опубликовано',
    },
    {
        value: 'draft',
        label: 'Черновик',
    },
    {
        value: 'archived',
        label: 'Архив',
    },
] as const satisfies IOption[];

export type ObjectStatus = (typeof OBJECT_STATUS_OPTIONS)[number]['value'];
