import { NgTemplateOutlet } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    DestroyRef,
    inject,
    input,
    InputSignal,
    signal,
    WritableSignal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ReactiveFormsModule } from '@angular/forms';
import { MediaImage } from '@project-data-hub/modules/media';
import { IOption } from '@project-data-hub/shared';
import { TuiIdentityMatcher, TuiStringHandler } from '@taiga-ui/cdk';
import { TuiButton, TuiDialogService, TuiError, TuiInput } from '@taiga-ui/core';
import {
    TuiAccordion,
    TuiChevron,
    TuiDataListWrapper,
    TuiInputChip,
    TuiInputNumber,
    TuiInputYear,
    TuiSelect,
    TuiStepper,
    TuiTextarea,
} from '@taiga-ui/kit';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';

import { ObjectImageModalComponent } from '../object-image-modal/object-image-modal.component';
import { ObjectFormViewModel } from './view-models/object-form.view-model';

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
        TuiInputNumber,
        TuiInputChip,
        TuiAccordion,
    ]
})
export class ObjectFormComponent {
    public readonly model: InputSignal<ObjectFormViewModel> = input.required();
    public readonly isReadonly: InputSignal<boolean> = input(false);

    protected readonly activeStepIndex: WritableSignal<number> = signal(0);

    private readonly _dialogService: TuiDialogService = inject(TuiDialogService);
    private readonly _destroyRef: DestroyRef = inject(DestroyRef);

    protected readonly optionStringify: TuiStringHandler<IOption> = (option) => option.label;
    protected readonly optionMatcher: TuiIdentityMatcher<IOption> = (a, b) => a.value === b.value;

    protected goToNextStep(): void {
        if (!this.model().isValid) {
            this.model().markInvalidAsTouched();
        } else {
            this.activeStepIndex.update((value) => value + 1);
        }
    }

    protected openImageModal(fileType: string): void {
        this._dialogService.open<MediaImage>(new PolymorpheusComponent(ObjectImageModalComponent), {
            label: 'Добавить изображение',
            size: 'l',
            data: {
                fileType
            }
        })
        .pipe(
            takeUntilDestroyed(this._destroyRef)
        )
        .subscribe();
    }
}
