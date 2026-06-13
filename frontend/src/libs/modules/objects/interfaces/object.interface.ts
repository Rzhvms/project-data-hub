import { ObjectStage } from '../constants/object-stage.constant';
import { IObjectIndicators } from './object-indicators.interface';
import { IObjectMedia } from './object-media.interface';
import { IObjectPreview } from './object-preview.interface';
import { IObjectTeam } from './object-team.interface';

export interface IObject extends IObjectPreview {
    shortTitle?: string;
    shortDescription: string;
    fullDescription?: string;
    designYear?: number;
    implementationYear?: number;
    stage: ObjectStage;
    customer?: string;
    media: IObjectMedia;
    indicators: IObjectIndicators;
    team: IObjectTeam;
}
