import { FormControl, FormGroup, Validators } from '@angular/forms';
import { OBJECT_TYPE_OPTIONS, OBJECT_STATUS_OPTIONS, OBJECT_STAGE_OPTIONS, ObjectType, ObjectStatus, ObjectStage, IObject } from '@project-data-hub/modules/objects';
import { IOption } from '@project-data-hub/shared';

type ValidationState = {
    isValid: boolean;
    invalidForms: FormGroup[]
}

type MainForm = {
    title: FormControl<string>;
    shortTitle: FormControl<string>;
    type: FormControl<ObjectType | null>;
    city: FormControl<string>;
    customer: FormControl<string>;
    fullDescription: FormControl<string>;
    shortDescription: FormControl<string>;
    stage: FormControl<ObjectStage | null>;
    designYear: FormControl<number | null>;
    implementationYear: FormControl<number | null>;
};

type IndicatorsForm = {
    totalArea: FormControl<number | null>;
    plotArea: FormControl<number | null>;
    buildingArea: FormControl<number | null>;
    sectionsCount: FormControl<number | null>;
    floorsCount: FormControl<number | null>;
    roomsCount: FormControl<number | null>;
    parkingSpacesCount: FormControl<number | null>;
};

export class ObjectFormViewModel {
    public get isValid(): boolean {
        return this._validationState.isValid;
    }

    private get _validationState(): ValidationState {
        const invalidForms: FormGroup[] = [this.mainForm, this.indicatorsForm]
            .filter((form) => form.invalid)

        return {
            isValid: invalidForms.length === 0,
            invalidForms
        }
    }

    public readonly stepList: string[] = [
        'Основные',
        'Медиа',
        'Размеры',
        'Команда'
    ];
    public readonly objectTypeList: IOption[] = OBJECT_TYPE_OPTIONS;
    public readonly objectStageList: IOption[] = OBJECT_STAGE_OPTIONS;

    public readonly mainForm: FormGroup<MainForm> = new FormGroup<MainForm>({
        title: new FormControl('', {
            nonNullable: true,
            validators: [Validators.required]
        }),
        shortTitle: new FormControl('', {
            nonNullable: true
        }),
        type: new FormControl<ObjectType | null>(null, {
            validators: [Validators.required]
        }),
        city: new FormControl('', {
            nonNullable: true,
            validators: [Validators.required]
        }),
        customer: new FormControl('', {
            nonNullable: true
        }),
        fullDescription: new FormControl('', {
            nonNullable: true,
            validators: [Validators.maxLength(300)]
        }),
        shortDescription: new FormControl('', {
            nonNullable: true
        }),
        stage: new FormControl<ObjectStage | null>(null, {
            validators: [Validators.required]
        }),
        designYear: new FormControl<number | null>(null),
        implementationYear: new FormControl<number | null>(null)
    });

    public readonly indicatorsForm: FormGroup<IndicatorsForm> = new FormGroup<IndicatorsForm>({
        totalArea: new FormControl<number | null>(null, {
            validators: [Validators.required]
        }),
        plotArea: new FormControl<number | null>(null, {
            validators: [Validators.required]
        }),
        buildingArea: new FormControl<number | null>(null, {
            validators: [Validators.required]
        }),
        sectionsCount: new FormControl<number | null>(null, {
            validators: [Validators.required]
        }),
        floorsCount: new FormControl<number | null>(null),
        roomsCount: new FormControl<number | null>(null),
        parkingSpacesCount: new FormControl<number | null>(null),
    });

    public markInvalidAsTouched(): void {
        this._validationState.invalidForms.forEach((form) => form.markAllAsTouched());
    }

    public updateModel(value: IObject): void {
        this.mainForm.patchValue({
            title: value.title,
            shortTitle: value.shortTitle,
            type: value.type,
            city: value.city,
            customer: value.customer,
            fullDescription: value.fullDescription,
            shortDescription: value.shortDescription,
            stage: value.stage,
            designYear: value.designYear,
            implementationYear: value.implementationYear
        }, { emitEvent: false });
    }
}
