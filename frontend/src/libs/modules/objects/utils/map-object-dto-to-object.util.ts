import { MediaImage } from '@project-data-hub/modules/media';

import { IObject } from '../interfaces/object.interface';
import { IObjectDto } from '../interfaces/object-dto.interface';
import { IMediaImageDto } from '../interfaces/object-media-dto.interface';

export function mapObjectDtoToObject(dto: IObjectDto): IObject {
    return {
        ...dto,
        media: {
            mainImage: dto.media.mainImage
                ? mapMediaImageDtoToMediaImage(dto.media.mainImage)
                : null,
            images: dto.media.images?.map(mapMediaImageDtoToMediaImage),
            schemas: dto.media.schemas?.map(mapMediaImageDtoToMediaImage),
            renders: dto.media.renders?.map(mapMediaImageDtoToMediaImage),
            photos: dto.media.photos?.map(mapMediaImageDtoToMediaImage),
            presentationCover: dto.media.presentationCover
                ? mapMediaImageDtoToMediaImage(dto.media.presentationCover)
                : undefined,
            portfolioImages: dto.media.portfolioImages?.map(mapMediaImageDtoToMediaImage),
        },
    };
}

function mapMediaImageDtoToMediaImage(dto: IMediaImageDto): MediaImage {
    return {
        id: dto.id,
        file: new File([], dto.url ?? ''),
        title: dto.title,
        description: dto.description,
        alternativeText: dto.alternativeText,
        useInSite: dto.useInSite,
        useInPresentation: dto.useInPresentation,
        useInPortfolio: dto.useInPortfolio,
    };
}
