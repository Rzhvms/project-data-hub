import { IOption } from '@project-data-hub/shared';

export const OBJECT_TYPE_OPTIONS = [
    {
        value: 'housingComplex',
        label: 'Жилые комплексы',
    },
    {
        value: 'publicBuilding',
        label: 'Общественные здания',
    },
    {
        value: 'commercialRealEstate',
        label: 'Коммерческая недвижимость',
    },
    {
        value: 'landscaping',
        label: 'Благоустройство',
    },
    {
        value: 'urbanPlanning',
        label: 'Градостроительство',
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
