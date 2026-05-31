import { ChangeDetectionStrategy, Component, inject, signal, WritableSignal } from '@angular/core';
import { IObjectPreview, ObjectsRequestService } from '@project-data-hub/modules/objects';
import { TuiButton, TuiDropdown, TuiInput } from '@taiga-ui/core';
import { take } from 'rxjs';

import { ObjectsPageTableComponent } from './components/objects-page-table/objects-page-table.component';
import { ObjectsPageToolBarComponent } from './components/objects-page-tool-bar/objects-page-tool-bar.component';
import { ObjectsPageToolBarViewModel } from './components/objects-page-tool-bar/view-models/objects-page-tool-bar.view-model';

@Component({
    selector: 'objects-page',
    templateUrl: './objects.page.html',
    styleUrl: './styles/objects.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiInput,
        TuiDropdown,
        ObjectsPageToolBarComponent,
        ObjectsPageTableComponent
    ]
})
export class ObjectsPageComponent {
    protected readonly objectList: WritableSignal<IObjectPreview[]> = signal([]);
    protected readonly toolBarViewModel: ObjectsPageToolBarViewModel = new ObjectsPageToolBarViewModel();

    private readonly _requestService: ObjectsRequestService = inject(ObjectsRequestService);

    constructor() {
        this._requestService.getObjectList()
            .pipe(take(1))
            .subscribe((objectList) => this.objectList.set(objectList));
    }
}
