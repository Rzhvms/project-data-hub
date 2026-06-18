import {
    ChangeDetectionStrategy,
    Component,
    DestroyRef,
    inject,
    OnDestroy,
    signal,
    WritableSignal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import {
    MediaDetailModalComponent,
    MediaImage,
    MediaRequestService,
    MediaUploadModalComponent,
} from '@project-data-hub/modules/media';
import { TuiButton, TuiDialogService } from '@taiga-ui/core';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { debounceTime, take } from 'rxjs';

import { MediaPageToolBarComponent } from './components/media-page-tool-bar/media-page-tool-bar.component';
import { MediaPageToolBarViewModel } from './components/media-page-tool-bar/view-models/media-page-tool-bar.view-model';

@Component({
    templateUrl: './media.page.html',
    styleUrl: './styles/media.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    host: {
        style: 'display: block; height: 100%;',
    },
    imports: [TuiButton, MediaPageToolBarComponent],
})
export class MediaPageComponent implements OnDestroy {
    protected readonly images: WritableSignal<MediaImage[]> = signal([]);
    protected readonly filteredImages: WritableSignal<MediaImage[]> = signal([]);
    protected readonly toolBarViewModel: MediaPageToolBarViewModel =
        new MediaPageToolBarViewModel();

    private readonly _requestService: MediaRequestService = inject(MediaRequestService);
    private readonly _dialogService: TuiDialogService = inject(TuiDialogService);
    private readonly _sanitizer: DomSanitizer = inject(DomSanitizer);
    private readonly _destroyRef: DestroyRef = inject(DestroyRef);
    private readonly _objectUrls: Map<string, string> = new Map<string, string>();

    constructor() {
        this.loadMedia();
        this.initFilters();
        this.initChanges();
    }

        public ngOnDestroy(): void {
        for (const url of this._objectUrls.values()) {
            URL.revokeObjectURL(url);
        }

        this._objectUrls.clear();
    }

    protected loadMedia(): void {
        this._requestService
            .getMediaImages()
            .pipe(take(1))
            .subscribe((images: MediaImage[]) => {
                this.images.set([...images]);
                this.filteredImages.set([...images]);
            });
    }

    protected getImageUrl(file: File): string {
        const cached: string | undefined = this._objectUrls.get(file.name);

        if (cached) {
            return cached;
        }

        const url: string = URL.createObjectURL(file);
        this._objectUrls.set(file.name, url);

        return url;
    }

    protected openDetail(image: MediaImage): void {
        const url: string = this.getImageUrl(image.file);
        const safeUrl: SafeUrl = this._sanitizer.bypassSecurityTrustUrl(url);

        Promise.resolve().then(() => {
            this._dialogService
                .open(new PolymorpheusComponent(MediaDetailModalComponent), {
                    label: image.title,
                    size: 'l',
                    data: { image, url: safeUrl },
                })
                .pipe(take(1))
                .subscribe();
        });
    }

    protected openUpload(): void {
        Promise.resolve().then(() => {
            this._dialogService
                .open<MediaImage>(new PolymorpheusComponent(MediaUploadModalComponent), {
                    label: 'Добавить изображение',
                    size: 'l',
                })
                .pipe(take(1))
                .subscribe((result: MediaImage | undefined) => {
                    if (result) {
                        this._requestService.uploadMedia(result.file, result)
                            .pipe(take(1))
                            .subscribe();
                    }
                });
        });
    }

    protected trackById(index: number, image: MediaImage): string {
        return image.id;
    }

    private initChanges(): void {
        this._requestService.changes$
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe(() => this.loadMedia());
    }

    private initFilters(): void {
        this.toolBarViewModel.form.valueChanges
            .pipe(debounceTime(100), takeUntilDestroyed(this._destroyRef))
            .subscribe(() => {
                const filterValue = this.toolBarViewModel.form.getRawValue();
                const query: string = filterValue.search.toLowerCase().trim();
                const usageFilter: string[] = filterValue.usageFilter;

                const result: MediaImage[] = this.images().filter((image: MediaImage) => {
                    if (
                        query &&
                        !image.title.toLowerCase().includes(query) &&
                        !image.description.toLowerCase().includes(query) &&
                        !(image.alternativeText ?? '').toLowerCase().includes(query)
                    ) {
                        return false;
                    }

                    if (usageFilter.includes('На сайте') && !image.useInSite) {
                        return false;
                    }

                    if (usageFilter.includes('В презентации') && !image.useInPresentation) {
                        return false;
                    }

                    if (usageFilter.includes('В портфолио') && !image.useInPortfolio) {
                        return false;
                    }

                    return true;
                });

                this.filteredImages.set(result);
            });
    }
}
