import { ObjectStage } from '../constants/object-stage.constant';
import { IObjectIndicators } from './object-indicators.interface';
import { IObjectMediaDto } from './object-media-dto.interface';
import { IObjectPreview } from './object-preview.interface';
import { IObjectTeam } from './object-team.interface';

export interface IObjectDto extends IObjectPreview {
    shortTitle?: string;
    shortDescription: string;
    fullDescription?: string;
    designYear?: number;
    implementationYear?: number;
    stage: ObjectStage;
    customer?: string;
    media: IObjectMediaDto;
    indicators: IObjectIndicators;
    team: IObjectTeam;
}
