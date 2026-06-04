import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ObjectsRequestService } from '@project-data-hub/modules/objects';

@Component({
    templateUrl: './object-view.page.html',
    styleUrl: './styles/object-view.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ObjectViewPageComponent {
    private readonly _requestService: ObjectsRequestService = inject(ObjectsRequestService);
}
