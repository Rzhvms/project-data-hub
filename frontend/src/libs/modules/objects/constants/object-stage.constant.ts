import { IOption } from '@project-data-hub/shared';

export const OBJECT_STAGE_OPTIONS = [
    {
        value: 'preDesign',
        label: 'Предпроектная проработка',
    },
    {
        value: 'concept',
        label: 'Концепция',
    },
    {
        value: 'feasibilityStudy',
        label: 'Технико-экономическое обоснование',
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
        value: 'expertReview',
        label: 'Экспертиза',
    },
    {
        value: 'tender',
        label: 'Тендер',
    },
    {
        value: 'construction',
        label: 'Строительство',
    },
    {
        value: 'authorSupervision',
        label: 'Авторский надзор',
    },
    {
        value: 'commissioning',
        label: 'Ввод в эксплуатацию',
    },
    {
        value: 'completed',
        label: 'Реализован',
    },
    {
        value: 'renovation',
        label: 'Реконструкция',
    },
] as const satisfies IOption[];

export type ObjectStage = (typeof OBJECT_STAGE_OPTIONS)[number]['value'];
