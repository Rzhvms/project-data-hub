import { IOption } from '../interfaces';

export const optionMatcher = (a: IOption, b: IOption): boolean => a.value === b.value;
