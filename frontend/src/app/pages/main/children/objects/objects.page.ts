import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'objects-page',
    templateUrl: './objects.page.html',
    styleUrl: './styles/objects.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ObjectsPageComponent {}
