import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IMediaImageDto } from '@project-data-hub/modules/objects';
import { DbCollectionConfig, DtoMapperService } from '@project-data-hub/shared';
import { forkJoin, Observable, of, Subject } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';

import { MediaImage } from '../types/media-image.type';

const MEDIA_COLLECTION: DbCollectionConfig = {
    name: 'media-images',
    seedUrl: '/mocks/media-images.mock.json',
};

@Injectable({ providedIn: 'root' })
export class MediaRequestService {
    public readonly changes$: Observable<void>;
    private readonly _http: HttpClient = inject(HttpClient);
    private readonly _db: DtoMapperService = inject(DtoMapperService);
    private readonly _changes: Subject<void> = new Subject<void>();

    constructor() {
        this.changes$ = this._changes;
    }

    public getMediaImages(): Observable<MediaImage[]> {
        return this._db
            .getCollection<IMediaImageDto>(MEDIA_COLLECTION)
            .pipe(switchMap((dtos: IMediaImageDto[]) => this.fetchImages(dtos)));
    }

    public uploadMedia(file: File, metadata: Partial<IMediaImageDto>): Observable<MediaImage> {
        const id = `media-${Date.now()}`;
        const url: string = URL.createObjectURL(file);
        const dto: IMediaImageDto = {
            id,
            url,
            title: metadata.title ?? file.name,
            description: metadata.description ?? '',
            alternativeText: metadata.alternativeText ?? '',
            useInSite: metadata.useInSite ?? false,
            useInPresentation: metadata.useInPresentation ?? false,
            useInPortfolio: metadata.useInPortfolio ?? false,
        };

        return this._db.create(MEDIA_COLLECTION, dto).pipe(
            tap(() => this._changes.next()),
            map(() => ({
                id,
                file,
                title: dto.title,
                description: dto.description,
                alternativeText: dto.alternativeText,
                useInSite: dto.useInSite,
                useInPresentation: dto.useInPresentation,
                useInPortfolio: dto.useInPortfolio,
            })),
        );
    }

    public deleteMedia(id: string): Observable<boolean> {
        return this._db.delete(MEDIA_COLLECTION, id).pipe(tap(() => this._changes.next()));
    }

    public updateMedia(
        id: string,
        changes: Partial<IMediaImageDto>,
    ): Observable<MediaImage | null> {
        return this._db.update<IMediaImageDto>(MEDIA_COLLECTION, id, changes).pipe(
            tap(() => this._changes.next()),
            map((dto: IMediaImageDto | null) => {
                if (!dto) {
                    return null;
                }

                return {
                    id: dto.id,
                    file: new File([], dto.url ?? ''),
                    title: dto.title,
                    description: dto.description,
                    alternativeText: dto.alternativeText,
                    useInSite: dto.useInSite,
                    useInPresentation: dto.useInPresentation,
                    useInPortfolio: dto.useInPortfolio,
                } as MediaImage;
            }),
        );
    }

    private fetchImages(dtos: IMediaImageDto[]): Observable<MediaImage[]> {
        const entries: Array<{ dto: IMediaImageDto; url: string }> = [];

        for (const dto of dtos) {
            if (dto.url) {
                entries.push({ dto, url: dto.url });
            }
        }

        if (entries.length === 0) {
            return of(dtos.map((dto) => this.mapToMediaImage(dto)));
        }

        return forkJoin(
            entries.map(({ url }) =>
                this._http.get(url, { responseType: 'blob' }).pipe(
                    map((blob: Blob): [string, Blob] => [url, blob]),
                    catchError((): Observable<[string, null]> => of([url, null])),
                ),
            ),
        ).pipe(
            map((results: Array<[string, Blob | null]>) => {
                const urlToBlob = new Map<string, Blob>();

                for (const [url, blob] of results) {
                    if (blob) {
                        urlToBlob.set(url, blob);
                    }
                }

                return dtos.map((dto) => this.mapToMediaImage(dto, urlToBlob));
            }),
        );
    }

    private mapToMediaImage(dto: IMediaImageDto, urlToBlob?: Map<string, Blob>): MediaImage {
        const blob: Blob | undefined = dto.url ? urlToBlob?.get(dto.url) : undefined;

        return {
            id: dto.id,
            file: blob
                ? new File([blob], this.extractFileName(dto.url!), { type: blob.type })
                : new File([], dto.url ?? ''),
            title: dto.title,
            description: dto.description,
            alternativeText: dto.alternativeText,
            useInSite: dto.useInSite,
            useInPresentation: dto.useInPresentation,
            useInPortfolio: dto.useInPortfolio,
        };
    }

    private extractFileName(url: string): string {
        return url.split('/').pop() ?? 'image';
    }
}
