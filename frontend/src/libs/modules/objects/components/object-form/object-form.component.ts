import { NgTemplateOutlet } from '@angular/common';
import { ChangeDetectionStrategy, Component, input, InputSignal, signal, WritableSignal } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiError, TuiInput, tuiItemsHandlersProvider } from '@taiga-ui/core';
import { TuiChevron, TuiDataListWrapper, TuiInputNumber, TuiInputYear, TuiSelect, TuiStepper, TuiTextarea } from '@taiga-ui/kit';

import { ObjectFormViewModel } from './view-models/object-form.view-model';
import { IOption } from '@project-data-hub/shared';

@Component({
    selector: 'object-form',
    templateUrl: './object-form.component.html',
    styleUrl: './styles/object-form.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiStepper,
        TuiButton,
        TuiSelect,
        TuiChevron,
        TuiDataListWrapper,
        TuiTextarea,
        TuiInput,
        TuiInputYear,
        NgTemplateOutlet,
        ReactiveFormsModule,
        TuiError,
        TuiInputNumber
    ],
    providers: [
        tuiItemsHandlersProvider({
            stringify: signal((item: IOption) => item.label),
            identityMatcher: signal((a: IOption, b: IOption) => a.value === b.value)
        })
    ]
})
export class ObjectFormComponent {
    public readonly model: InputSignal<ObjectFormViewModel> = input.required();
    public readonly isReadonly: InputSignal<boolean> = input(false);

    protected readonly activeStepIndex: WritableSignal<number> = signal(0);

    protected goToNextStep(): void {
        if (!this.model().isValid) {
            this.model().markInvalidAsTouched();
        } else {
            this.activeStepIndex.update((value) => value += 1);
        }
    }
}
