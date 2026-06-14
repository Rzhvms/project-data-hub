import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IObjectFormValue, ObjectFormComponent } from '@project-data-hub/modules/objects';
import { TuiButton } from '@taiga-ui/core';
import { take, tap } from 'rxjs';

import { ObjectChildBasePageComponent } from '../base/object-child.base.page';

@Component({
    templateUrl: './object-edit.page.html',
    styleUrl: './styles/object-edit.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        ObjectFormComponent
    ]
})
export class ObjectEditPageComponent extends ObjectChildBasePageComponent {
    private readonly _objectId: string | undefined = inject(ActivatedRoute).snapshot.params['objectId']

    constructor() {
        super();
        if (!this._objectId) {
            return;
        }

        this.requestService.getObjectById(this._objectId)
            .pipe(
                tap((value) => this.formViewModel.updateModel(value)),
                take(1)
            )
            .subscribe();
    }

    protected saveAsDraft(): void {
            const value: Partial<IObjectFormValue> = this.formViewModel.fromModelToDraft();
            console.log(value);
        }

    protected publish(): void {
        const invalidStep: number | null = this.formViewModel.validateAllAndGetFirstInvalidStep();
        if (invalidStep !== null) {
            this.formViewModel.activeStepIndex.set(invalidStep);

            return;
        }
        const value: IObjectFormValue = this.formViewModel.fromModel();
        console.log(value);
    }
}
