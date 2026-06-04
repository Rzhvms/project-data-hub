export type MediaImage = {
    id: string;
    file: File;
    title: string;
    description: string;
    alternativeText?: string;
    useInSite: boolean;
    useInPresentation: boolean;
    useInPortfolio: boolean;
};
