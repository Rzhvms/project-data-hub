import { ObjectStatus } from '../constants/object-status.constant';

export function getObjectUiStatus(status: ObjectStatus): 'positive' | 'warning' | 'neutral' {
    switch(status) {
        case 'published':
            return 'positive';
        case 'draft':
            return 'warning';
        case 'archived':
            return 'neutral';
    };
}
