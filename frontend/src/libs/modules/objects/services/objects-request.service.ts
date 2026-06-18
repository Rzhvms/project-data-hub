import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { MediaImage } from '@project-data-hub/modules/media';
import { DbCollectionConfig, DtoMapperService } from '@project-data-hub/shared';
import { forkJoin, Observable, of, Subject, take, throwError } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';

import { ObjectStatus } from '../constants/object-status.constant';
import { IObject } from '../interfaces/object.interface';
import { IObjectDto } from '../interfaces/object-dto.interface';
import { IObjectFormValue } from '../interfaces/object-form-value.interface';
import { IMediaImageDto, IObjectMediaDto } from '../interfaces/object-media-dto.interface';
import { IObjectPreview } from '../interfaces/object-preview.interface';

const PREVIEW_COLLECTION: DbCollectionConfig = {
    name: 'objects-preview',
    seedUrl: '/mocks/objects-preview.mock.json',
};

const DETAIL_COLLECTION_CONFIG: DbCollectionConfig = {
    name: 'objects-detail',
    seedUrl: '',
};

@Injectable()
export class ObjectsRequestService {
    private readonly _http: HttpClient = inject(HttpClient);
    private readonly _db: DtoMapperService = inject(DtoMapperService);
    private readonly _changes: Subject<void> = new Subject<void>();

    private _detailSeeded = false;

    public readonly changes$: Observable<void> = this._changes.asObservable();

    public getObjectList(): Observable<IObjectPreview[]> {
        return this._db.getCollection<IObjectPreview>(PREVIEW_COLLECTION);
    }

    public getObjectById(id: string): Observable<IObject> {
        return this._ensureDetailSeeded().pipe(
            switchMap(() => this._db.getById<IObjectDto>(DETAIL_COLLECTION_CONFIG, id)),
            switchMap((dto: IObjectDto | null) => {
                if (dto) {
                    return this.fetchAndAssemble(dto);
                }

                return this._db.getById<IObjectPreview>(PREVIEW_COLLECTION, id).pipe(
                    switchMap((preview: IObjectPreview | null) => {
                        if (!preview) {
                            return throwError(() => new Error(`Object ${id} not found`));
                        }

                        const minimalDto = this._previewToMinimalDto(preview);
                        return this._db.create(DETAIL_COLLECTION_CONFIG, minimalDto).pipe(
                            switchMap(() => this.fetchAndAssemble(minimalDto)),
                        );
                    }),
                );
            }),
        );
    }

    public createObject(
        formValue: IObjectFormValue,
        status: ObjectStatus,
    ): Observable<IObject> {
        const now = new Date().toISOString();
        const id = `obj-${Date.now()}`;

        const dto: IObjectDto = {
            id,
            title: formValue.title,
            city: formValue.city,
            type: formValue.type,
            status,
            projectManager: formValue.projectManager,
            createdAt: now,
            updatedAt: now,
            shortDescription: formValue.shortDescription,
            shortTitle: formValue.shortTitle,
            fullDescription: formValue.fullDescription,
            designYear: formValue.designYear,
            implementationYear: formValue.implementationYear,
            stage: formValue.stage,
            customer: formValue.customer,
            media: {
                mainImage: this._mediaImageToDto(formValue.media.mainImage),
                images: this._mediaImagesToDto(formValue.media.images),
                schemas: this._mediaImagesToDto(formValue.media.schemas),
                renders: this._mediaImagesToDto(formValue.media.renders),
                photos: this._mediaImagesToDto(formValue.media.photos),
                presentationCover: this._mediaImageToDto(formValue.media.presentationCover),
                portfolioImages: this._mediaImagesToDto(formValue.media.portfolioImages),
            },
            indicators: {
                totalArea: formValue.indicators.totalArea ?? 0,
                plotArea: formValue.indicators.plotArea ?? 0,
                buildingArea: formValue.indicators.buildingArea ?? 0,
                sectionsCount: formValue.indicators.sectionsCount ?? 0,
                floorsCount: formValue.indicators.floorsCount,
                roomsCount: formValue.indicators.roomsCount,
                parkingSpacesCount: formValue.indicators.parkingSpacesCount,
            },
            team: {
                chiefArchitect: formValue.team.chiefArchitect,
                chiefEngineer: formValue.team.chiefEngineer,
                architects: formValue.team.architects ?? [],
                engineers: formValue.team.engineers ?? [],
                bimSpecialists: formValue.team.bimSpecialists ?? [],
                visualizers: formValue.team.visualizers ?? [],
                partners: formValue.team.partners ?? [],
            },
        };

        return this._ensureDetailSeeded().pipe(
            switchMap(() => forkJoin([
                this._db.create(PREVIEW_COLLECTION, {
                    id,
                    title: dto.title,
                    city: dto.city,
                    type: dto.type,
                    status: dto.status,
                    projectManager: dto.projectManager,
                    createdAt: dto.createdAt,
                    updatedAt: dto.updatedAt,
                } as IObjectPreview),
                this._db.create(DETAIL_COLLECTION_CONFIG, dto),
            ])),
            tap(() => this._changes.next()),
            switchMap(() => this.fetchAndAssemble(dto)),
        );
    }

    public updateObject(
        id: string,
        formValue: Partial<IObjectFormValue>,
        newStatus?: ObjectStatus,
    ): Observable<IObject | null> {
        return this._ensureDetailSeeded().pipe(
            switchMap(() => this._db.getById<IObjectDto>(DETAIL_COLLECTION_CONFIG, id)),
            switchMap((existing: IObjectDto | null) => {
                if (!existing) {
                    return throwError(() => new Error(`Object ${id} not found`));
                }

                const now = new Date().toISOString();
                const updatedDto: IObjectDto = {
                    ...existing,
                    ...formValue,
                    status: newStatus ?? existing.status,
                    updatedAt: now,
                    media: existing.media,
                    indicators: {
                        ...existing.indicators,
                        ...formValue.indicators,
                    },
                    team: {
                        ...existing.team,
                        ...formValue.team,
                    },
                };

                return forkJoin([
                    this._db.update<IObjectPreview>(PREVIEW_COLLECTION, id, {
                        title: updatedDto.title,
                        city: updatedDto.city,
                        type: updatedDto.type,
                        status: updatedDto.status,
                        projectManager: updatedDto.projectManager,
                        updatedAt: now,
                    } as any),
                    this._db.update(DETAIL_COLLECTION_CONFIG, id, updatedDto),
                ]).pipe(
                    tap(() => this._changes.next()),
                    switchMap(() => this.fetchAndAssemble(updatedDto)),
                );
            }),
        );
    }

    public changeObjectStatus(
        id: string,
        newStatus: ObjectStatus,
    ): Observable<IObjectPreview | null> {
        return this._ensureDetailSeeded().pipe(
            switchMap(() =>
                this._db.update<IObjectPreview>(PREVIEW_COLLECTION, id, {
                    status: newStatus,
                    updatedAt: new Date().toISOString(),
                } as any)
            ),
            tap(() => {
                this._db
                    .update(DETAIL_COLLECTION_CONFIG, id, {
                        status: newStatus,
                        updatedAt: new Date().toISOString(),
                    } as any)
                    .pipe(take(1))
                    .subscribe();
                this._changes.next();
            }),
        );
    }

    public exportFile(id: string, format: 'pptx' | 'docx'): Observable<Blob> {
        return this._http.get(`/mocks/mock-files/${id}.${format}`, { responseType: 'blob' });
    }

    private _ensureDetailSeeded(): Observable<void> {
        if (this._detailSeeded) {
            return of(void 0);
        }

        return this._seedDetailCache();
    }

    private _seedDetailCache(): Observable<void> {
        const persisted = localStorage.getItem('data-hub-mock:objects-detail');

        if (persisted) {
            try {
                const data = JSON.parse(persisted) as IObjectDto[];
                this._db.seedCollection('objects-detail', data);
                this._detailSeeded = true;
                return of(void 0);
            } catch {
                /* fall through */
            }
        }

        return forkJoin([
            this._http.get<IObjectDto>('/mocks/object-draft.mock.json'),
            this._http.get<IObjectDto>('/mocks/object-published.mock.json'),
            this._http.get<IObjectDto>('/mocks/object-archived.mock.json'),
            this.getObjectList(),
        ]).pipe(
            tap(([draft, published, archived, previews]: [IObjectDto, IObjectDto, IObjectDto, IObjectPreview[]]) => {
                const detailMap = new Map<string, IObjectDto>();
                detailMap.set(draft.id, draft);
                detailMap.set(published.id, published);
                detailMap.set(archived.id, archived);

                for (const preview of previews) {
                    if (!detailMap.has(preview.id)) {
                        detailMap.set(preview.id, this._previewToMinimalDto(preview));
                    }
                }

                this._db.seedCollection('objects-detail', [...detailMap.values()]);
                this._detailSeeded = true;
            }),
            map(() => void 0),
            catchError(() => {
                this._detailSeeded = true;
                return of(void 0);
            }),
        );
    }

    private _previewToMinimalDto(preview: IObjectPreview): IObjectDto {
        return {
            ...preview,
            shortDescription: '',
            stage: 'concept' as any,
            media: {
                mainImage: null,
                images: [],
                schemas: [],
                renders: [],
                photos: [],
                presentationCover: null,
                portfolioImages: [],
            },
            indicators: {
                totalArea: 0,
                plotArea: 0,
                buildingArea: 0,
                sectionsCount: 0,
            },
            team: {},
        };
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
            file:
                imgDto.url && urlToBlob.has(imgDto.url)
                    ? new File([urlToBlob.get(imgDto.url)!], this.extractFileName(imgDto.url), {
                          type: urlToBlob.get(imgDto.url)!.type,
                      })
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

    private _mediaImageToDto(img: MediaImage | null | undefined): IMediaImageDto | null {
        if (!img) {
            return null;
        }

        return {
            id: img.id,
            url: img.file.size > 0 ? URL.createObjectURL(img.file) : null,
            title: img.title,
            description: img.description,
            alternativeText: img.alternativeText,
            useInSite: img.useInSite,
            useInPresentation: img.useInPresentation,
            useInPortfolio: img.useInPortfolio,
        };
    }

    private _mediaImagesToDto(images: MediaImage[] | undefined): IMediaImageDto[] {
        return (images ?? []).map((img) => this._mediaImageToDto(img)!).filter(Boolean);
    }
}
