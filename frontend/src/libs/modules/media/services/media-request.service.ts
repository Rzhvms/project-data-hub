import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IMediaImageDto } from '@project-data-hub/modules/objects';
import { forkJoin, Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

import { MediaImage } from '../types/media-image.type';

@Injectable()
export class MediaRequestService {
    private readonly _http: HttpClient = inject(HttpClient);

    public getMediaImages(): Observable<MediaImage[]> {
        return this._http.get<IMediaImageDto[]>('/mocks/media-images.mock.json').pipe(
            switchMap((dtos: IMediaImageDto[]) => this.fetchImages(dtos)),
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
            return of(dtos.map(dto => this.mapToMediaImage(dto)));
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

                return dtos.map(dto => this.mapToMediaImage(dto, urlToBlob));
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
