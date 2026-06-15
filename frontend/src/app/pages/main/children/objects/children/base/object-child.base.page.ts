import { Location } from '@angular/common';
import { Directive, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ObjectFormViewModel, ObjectsRequestService } from '@project-data-hub/modules/objects';
import { AppRoute } from '@project-data-hub/shared';

@Directive()
export class ObjectChildBasePageComponent {
    protected readonly formViewModel: ObjectFormViewModel = new ObjectFormViewModel();

    protected readonly requestService: ObjectsRequestService = inject(ObjectsRequestService);
    protected readonly router: Router = inject(Router);

    private readonly _location: Location = inject(Location);

    protected navigateBack(): void {
        const previousUrl = document.referrer;

        if (previousUrl.includes(AppRoute.ObjectsPage)) {
            this._location.back();

            return;
        }

        this.router.navigate([AppRoute.ObjectsPage]);
    }
}
