import { IOption } from '@project-data-hub/shared';

export const OBJECT_TYPE_OPTIONS = [
    {
        value: 'residence',
        label: 'Резиденция'
    },
    {
        value: 'commerce',
        label: 'Коммерция'
    },
    {
        value: 'housingComplex',
        label: 'Жилой комплекс'
    }
] as const satisfies readonly IOption[];

export type ObjectType = (typeof OBJECT_TYPE_OPTIONS)[number]['value'];
