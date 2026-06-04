import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ObjectsRequestService } from '@project-data-hub/modules/objects';

@Component({
    templateUrl: './object-create.page.html',
    styleUrl: './styles/object-create.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ObjectCreatePageComponent {
    private readonly _requestService: ObjectsRequestService = inject(ObjectsRequestService);
}
