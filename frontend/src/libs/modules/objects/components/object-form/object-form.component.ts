import { NgTemplateOutlet } from '@angular/common';
import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    DestroyRef,
    inject,
    input,
    InputSignal,
    signal,
    WritableSignal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MediaImage } from '@project-data-hub/modules/media';
import { IOption } from '@project-data-hub/shared';
import { TuiIdentityMatcher, TuiStringHandler } from '@taiga-ui/cdk';
import { TuiButton, TuiDialogService, TuiError, TuiInput } from '@taiga-ui/core';
import {
    TuiAccordion,
    TuiChevron,
    TuiDataListWrapper,
    TuiFiles,
    TuiInputChip,
    TuiInputNumber,
    TuiInputYear,
    TuiSelect,
    TuiStepper,
    TuiTextarea,
} from '@taiga-ui/kit';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { Observable, tap } from 'rxjs';

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
        TuiFiles,
    ]
})
export class ObjectFormComponent {
    public readonly model: InputSignal<ObjectFormViewModel> = input.required();
    public readonly isReadonly: InputSignal<boolean> = input(false);

    private readonly _isRemovingImage: WritableSignal<boolean> = signal(false);

    private readonly _dialogService: TuiDialogService = inject(TuiDialogService);
    private readonly _destroyRef: DestroyRef = inject(DestroyRef);
    private readonly _cdr: ChangeDetectorRef = inject(ChangeDetectorRef);

    protected readonly optionStringify: TuiStringHandler<IOption | null> = (option) => option?.label ?? '';
    protected readonly optionMatcher: TuiIdentityMatcher<IOption> = (a, b) => a.value === b.value;

    protected goToNextStep(): void {
        const step = this.model().activeStepIndex();
        if (!this.model().isStepValid(step)) {
            this.model().markStepAsTouched(step);

            return;
        }

        this.model().activeStepIndex.update((value) => value + 1);
    }

    protected openSingleImageModal(control: FormControl<MediaImage | null>): void {
        if (this._isRemovingImage()) {
            return;
        }

        this.openImageModal(control.value)
            .pipe(
                tap(this.handleImageResult(control)),
                takeUntilDestroyed(this._destroyRef)
            ).subscribe();
    }

    protected openImageModalForArray(
        control: FormControl<MediaImage[]>,
        value?: MediaImage,
    ): void {
        if (this._isRemovingImage()) {
            return;
        }

        this.openImageModal(value ?? null)
            .pipe(
                tap(this.handleImageResult(control, value)),
                takeUntilDestroyed(this._destroyRef)
            )
            .subscribe();
    }

    protected removeSingleImage(control: FormControl<MediaImage | null>): void {
        this._isRemovingImage.set(true);
        control.setValue(null);
        this._cdr.markForCheck();
        setTimeout(() => this._isRemovingImage.set(false));
    }

    protected removeImageFromArray(control: FormControl<MediaImage[]>, image: MediaImage): void {
        this._isRemovingImage.set(true);
        control.setValue(control.value.filter(item => item !== image));
        this._cdr.markForCheck();
        setTimeout(() => this._isRemovingImage.set(false));
    }

    private openImageModal(image: MediaImage | null): Observable<MediaImage | null> {
        return this._dialogService.open<MediaImage>(
            new PolymorpheusComponent(ObjectImageModalComponent),
            {
                label: 'Добавить изображение',
                size: 'l',
                data: image
            }
        );
    }

    private handleImageResult(
        control: FormControl<MediaImage | MediaImage[] | null>,
        value?: MediaImage
    ): (image: MediaImage | null) => void {
        return (image: MediaImage | null): void => {
            if (!image) {
                return;
            }

            const currentValue: MediaImage | MediaImage[] | null = control.value;
             if (Array.isArray(currentValue)) {
                control.setValue(
                    value
                        ? currentValue.map(item => item === value ? image : item)
                        : [...currentValue, image],
                );
            } else {
                control.setValue(image);
            }
            this._cdr.markForCheck();
        };
    }
}
