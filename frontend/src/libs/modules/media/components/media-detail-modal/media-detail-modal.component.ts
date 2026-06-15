import { ChangeDetectionStrategy, Component } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';
import { TuiButton, TuiDialogContext } from '@taiga-ui/core';
import { injectContext } from '@taiga-ui/polymorpheus';

import { MediaImage } from '../../types/media-image.type';

type MediaDetailData = {
    image: MediaImage;
    url: SafeUrl;
};

@Component({
    templateUrl: './media-detail-modal.component.html',
    styleUrl: './media-detail-modal.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [TuiButton],
})
export class MediaDetailModalComponent {
    protected readonly context: TuiDialogContext<void, MediaDetailData> = injectContext();
    protected readonly image: MediaImage = this.context.data.image;
    protected readonly imageUrl: SafeUrl = this.context.data.url;

    protected download(): void {
        const url: string = URL.createObjectURL(this.image.file);
        const anchor: HTMLAnchorElement = document.createElement('a');
        anchor.href = url;
        anchor.download = this.image.file.name;
        anchor.click();
        URL.revokeObjectURL(url);
    }

    protected close(): void {
        this.context.$implicit.complete();
    }
}
