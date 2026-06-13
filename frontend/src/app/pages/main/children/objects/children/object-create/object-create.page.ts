import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ObjectFormComponent, ObjectFormViewModel, ObjectsRequestService } from '@project-data-hub/modules/objects';
import { AppRoute } from '@project-data-hub/shared';
import { TuiButton } from '@taiga-ui/core';

@Component({
    templateUrl: './object-create.page.html',
    styleUrl: './styles/object-create.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        ObjectFormComponent
    ]
})
export class ObjectCreatePageComponent {
    protected readonly formViewModel: ObjectFormViewModel = new ObjectFormViewModel();

    private readonly _requestService: ObjectsRequestService = inject(ObjectsRequestService);
    private readonly _router: Router = inject(Router);

    protected navigateBack(): void {
        this._router.navigate([AppRoute.ObjectsPage]);
    }
}
