import { IOption } from '../interfaces';

export const createOptionLabelMap = <T extends PropertyKey>(
    options: ReadonlyArray<IOption<T>>
): Record<T, string> => {
    return Object.fromEntries(
        options.map(({ value, label }) => [value, label])
    ) as Record<T, string>;
};
