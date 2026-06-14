import { ChangeDetectionStrategy, Component } from '@angular/core';
import { IObjectFormValue, ObjectFormComponent } from '@project-data-hub/modules/objects';
import { TuiButton } from '@taiga-ui/core';

import { ObjectChildBasePageComponent } from '../base/object-child.base.page';

@Component({
    templateUrl: './object-create.page.html',
    styleUrl: './styles/object-create.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        ObjectFormComponent
    ]
})
export class ObjectCreatePageComponent extends ObjectChildBasePageComponent {
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
