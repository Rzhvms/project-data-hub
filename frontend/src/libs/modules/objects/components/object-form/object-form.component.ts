import { NgTemplateOutlet } from '@angular/common';
import { ChangeDetectionStrategy, Component, input, InputSignal, signal, WritableSignal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiError, TuiInput } from '@taiga-ui/core';
import { TuiStepper } from '@taiga-ui/kit';

import { ObjectFormViewModel } from './view-models/object-form.view-model';

@Component({
    selector: 'object-form',
    templateUrl: './object-form.component.html',
    styleUrl: './styles/object-form.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiStepper,
        TuiButton,
        TuiInput,
        NgTemplateOutlet,
        ReactiveFormsModule,
        TuiError
    ]
})
export class ObjectFormComponent {
    public readonly model: InputSignal<ObjectFormViewModel> = input.required();
    public readonly isReadonly: InputSignal<boolean> = input(false);

    protected readonly activeStepIndex: WritableSignal<number> = signal(0);
}
