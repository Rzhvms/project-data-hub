import { MediaImage } from '@project-data-hub/modules/media';

export interface IObjectMedia {
    mainImage: MediaImage | null;
    images?: MediaImage[];
    schemas?: MediaImage[];
    renders?: MediaImage[];
    photos?: MediaImage[];
    presentationCover?: MediaImage;
    portfolioImages?: MediaImage[];
}
