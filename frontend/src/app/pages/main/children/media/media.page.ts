import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'media-page',
    templateUrl: './media.page.html',
    styleUrl: './styles/media.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MediaPageComponent {}
