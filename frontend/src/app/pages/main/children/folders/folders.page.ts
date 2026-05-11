import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'folders-page',
    templateUrl: './folders.page.html',
    styleUrl: './styles/folders.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FoldersPageComponent {}
