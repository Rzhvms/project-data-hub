import { TuiStringHandler } from '@taiga-ui/cdk';

import { IOption } from '../interfaces';

export const optionStringify: TuiStringHandler<IOption> = (option: IOption): string => option.label;
