import { ChangeDetectionStrategy, Component, inject, signal, WritableSignal } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { TuiButton, TuiDialogContext } from '@taiga-ui/core';
import { TuiButtonLoading } from '@taiga-ui/kit';
import { injectContext } from '@taiga-ui/polymorpheus';
import { delay, take, tap } from 'rxjs';

import { MediaRequestService } from '../../services/media-request.service';
import { MediaImage } from '../../types/media-image.type';

type MediaDetailData = {
    image: MediaImage;
    url: SafeUrl;
};

@Component({
    templateUrl: './media-detail-modal.component.html',
    styleUrl: './media-detail-modal.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [TuiButton, TuiButtonLoading],
})
export class MediaDetailModalComponent {
    protected readonly context: TuiDialogContext<void, MediaDetailData> = injectContext();
    protected readonly image: MediaImage = this.context.data.image;
    protected readonly imageUrl: SafeUrl = this.context.data.url;
    protected readonly isDeleting: WritableSignal<boolean> = signal(false);

    private readonly _mediaService: MediaRequestService = inject(MediaRequestService);

    protected download(): void {
        const url: string = URL.createObjectURL(this.image.file);
        const anchor: HTMLAnchorElement = document.createElement('a');
        anchor.href = url;
        anchor.download = this.image.file.name;
        anchor.click();
        URL.revokeObjectURL(url);
    }

    protected delete(): void {
        this.isDeleting.set(true);

        this._mediaService.deleteMedia(this.image.id).pipe(
            delay(600),
            take(1),
            tap(() => {
                this.isDeleting.set(false);
                this.close();
            }),
        ).subscribe();
    }

    protected close(): void {
        this.context.$implicit.complete();
    }
}
