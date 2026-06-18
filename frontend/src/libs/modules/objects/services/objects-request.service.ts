import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MediaImage } from '@project-data-hub/modules/media';
import { forkJoin, Observable, of, throwError } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { IObject } from '../interfaces/object.interface';
import { IObjectDto } from '../interfaces/object-dto.interface';
import { IMediaImageDto, IObjectMediaDto } from '../interfaces/object-media-dto.interface';
import { IObjectPreview } from '../interfaces/object-preview.interface';

const OBJECT_MOCK_MAP: Record<string, string> = {
    'obj-001': '/mocks/object-draft.mock.json',
    'obj-002': '/mocks/object-published.mock.json',
    'obj-003': '/mocks/object-archived.mock.json',
};

@Injectable()
export class ObjectsRequestService {
    private readonly _http: HttpClient = inject(HttpClient);

    public getObjectList(): Observable<IObjectPreview[]> {
        return this._http.get<IObjectPreview[]>('/mocks/objects-preview.mock.json');
    }

    public getObjectById(id: string): Observable<IObject> {
        const url: string | undefined = OBJECT_MOCK_MAP[id];

        if (!url) {
            return throwError(() => new Error(`Object with id ${id} not found`));
        }

        return this._http.get<IObjectDto>(url).pipe(
            switchMap((dto: IObjectDto) => this.fetchAndAssemble(dto)),
        );
    }

    private fetchAndAssemble(dto: IObjectDto): Observable<IObject> {
        const imageUrls: string[] = this.collectImageUrls(dto.media);

        if (imageUrls.length === 0) {
            return of(this.assembleObject(dto, new Map()));
        }

        return forkJoin(
            imageUrls.map((imageUrl: string) =>
                this._http.get(imageUrl, { responseType: 'blob' }).pipe(
                    map((blob: Blob): [string, Blob] => [imageUrl, blob]),
                    catchError((): Observable<[string, null]> => of([imageUrl, null])),
                ),
            ),
        ).pipe(
            map((entries: Array<[string, Blob | null]>) => {
                const urlToBlob = new Map<string, Blob>();
                for (const [url, blob] of entries) {
                    if (blob) {
                        urlToBlob.set(url, blob);
                    }
                }

                return this.assembleObject(dto, urlToBlob);
            }),
        );
    }

    private collectImageUrls(media: IObjectMediaDto): string[] {
        const urls = new Set<string>();
        const addIfUrl = (img: IMediaImageDto | null | undefined): void => {
            if (img?.url) {
                urls.add(img.url);
            }
        };

        addIfUrl(media.mainImage);
        addIfUrl(media.presentationCover);
        media.images?.forEach(addIfUrl);
        media.schemas?.forEach(addIfUrl);
        media.renders?.forEach(addIfUrl);
        media.photos?.forEach(addIfUrl);
        media.portfolioImages?.forEach(addIfUrl);

        return [...urls];
    }

    private assembleObject(dto: IObjectDto, urlToBlob: Map<string, Blob>): IObject {
        const mapMediaImage = (imgDto: IMediaImageDto): MediaImage => ({
            id: imgDto.id,
            file: imgDto.url && urlToBlob.has(imgDto.url)
                ? new File(
                      [urlToBlob.get(imgDto.url)!],
                      this.extractFileName(imgDto.url),
                      { type: urlToBlob.get(imgDto.url)!.type },
                  )
                : new File([], imgDto.url ?? ''),
            title: imgDto.title,
            description: imgDto.description,
            alternativeText: imgDto.alternativeText,
            useInSite: imgDto.useInSite,
            useInPresentation: imgDto.useInPresentation,
            useInPortfolio: imgDto.useInPortfolio,
        });

        return {
            ...dto,
            media: {
                mainImage: dto.media.mainImage ? mapMediaImage(dto.media.mainImage) : null,
                images: dto.media.images?.map(mapMediaImage),
                schemas: dto.media.schemas?.map(mapMediaImage),
                renders: dto.media.renders?.map(mapMediaImage),
                photos: dto.media.photos?.map(mapMediaImage),
                presentationCover: dto.media.presentationCover
                    ? mapMediaImage(dto.media.presentationCover)
                    : undefined,
                portfolioImages: dto.media.portfolioImages?.map(mapMediaImage),
            },
        };
    }

    private extractFileName(url: string): string {
        return url.split('/').pop() ?? 'image';
    }

    public exportFile(id: string, format: 'pptx' | 'docx'): Observable<Blob> {
        return this._http.get(`/mocks/mock-files/${id}.${format}`, { responseType: 'blob' });
    }
}
