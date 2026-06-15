export interface IMediaImageDto {
    id: string;
    url: string | null;
    title: string;
    description: string;
    alternativeText?: string;
    useInSite: boolean;
    useInPresentation: boolean;
    useInPortfolio: boolean;
}

export interface IObjectMediaDto {
    mainImage: IMediaImageDto | null;
    images?: IMediaImageDto[];
    schemas?: IMediaImageDto[];
    renders?: IMediaImageDto[];
    photos?: IMediaImageDto[];
    presentationCover?: IMediaImageDto | null;
    portfolioImages?: IMediaImageDto[];
}
