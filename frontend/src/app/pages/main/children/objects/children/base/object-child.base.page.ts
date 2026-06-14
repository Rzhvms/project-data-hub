import { Directive, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ObjectFormViewModel, ObjectsRequestService } from '@project-data-hub/modules/objects';
import { AppRoute } from '@project-data-hub/shared';

@Directive()
export class ObjectChildBasePageComponent {
    protected readonly formViewModel: ObjectFormViewModel = new ObjectFormViewModel();

    protected readonly requestService: ObjectsRequestService = inject(ObjectsRequestService);
    private readonly _router: Router = inject(Router);

    protected navigateBack(): void {
        this._router.navigate([AppRoute.ObjectsPage]);
    }
}
