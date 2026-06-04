import { IOption } from '@project-data-hub/shared';

export const OBJECT_TYPE_OPTIONS = [
    {
        value: 'landscaping',
        label: 'Благоустройство',
    },
    {
        value: 'urbanPlanning',
        label: 'Градостроительство',
    },
    {
        value: 'housingComplex',
        label: 'Жилые комплексы',
    },
    {
        value: 'interior',
        label: 'Интерьер',
    },
    {
        value: 'concept',
        label: 'Концепция',
    },
    {
        value: 'commercialRealEstate',
        label: 'Коммерческая недвижимость',
    },
    {
        value: 'publicBuilding',
        label: 'Общественные здания',
    },
    {
        value: 'designDocumentation',
        label: 'Проектная документация',
    },
    {
        value: 'workingDocumentation',
        label: 'Рабочая документация',
    },
    {
        value: 'digitalTechnologies',
        label: 'Цифровые технологии',
    },
    {
        value: 'other',
        label: 'Другое',
    },
] as const satisfies IOption[];

export type ObjectType = (typeof OBJECT_TYPE_OPTIONS)[number]['value'];
