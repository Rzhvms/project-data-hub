import { environment } from '@env/environment';

import { ApiRoute } from '../enums';

export const getFullApiRoute = (route: ApiRoute): string => `${environment.apiHost}${route}`;
