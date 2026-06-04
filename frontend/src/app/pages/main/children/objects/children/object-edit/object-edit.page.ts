import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ObjectsRequestService } from '@project-data-hub/modules/objects';

@Component({
    templateUrl: './object-edit.page.html',
    styleUrl: './styles/object-edit.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ObjectEditPageComponent {
    private readonly _requestService: ObjectsRequestService = inject(ObjectsRequestService)
}
