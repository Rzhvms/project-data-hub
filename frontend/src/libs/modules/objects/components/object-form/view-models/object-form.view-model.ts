import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IObject, IObjectMedia,OBJECT_STAGE_OPTIONS, OBJECT_TYPE_OPTIONS, ObjectStage, ObjectStatus, ObjectType } from '@project-data-hub/modules/objects';
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

type TeamForm = {
    projectManager: FormControl<string>;
    chiefArchitect: FormControl<string>;
    chiefEngineer: FormControl<string>;
    architects: FormControl<string[]>;
    engineers: FormControl<string[]>;
    bimSpecialists: FormControl<string[]>;
    visualizers: FormControl<string[]>;
    partners: FormControl<string[]>;
};

export class ObjectFormViewModel {
    public get isValid(): boolean {
        return this._validationState.isValid;
    }

    private get _validationState(): ValidationState {
        const invalidForms: FormGroup[] = [
            this.mainForm,
            this.indicatorsForm,
            this.teamForm
        ].filter((form) => form.invalid);

        return {
            isValid: invalidForms.length === 0,
            invalidForms
        };
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
            validators: [Validators.required],
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
    public readonly teamForm: FormGroup<TeamForm> = new FormGroup<TeamForm>({
        projectManager: new FormControl('', {
            nonNullable: true,
            validators: [Validators.required]
        }),
        chiefArchitect: new FormControl('', {
            nonNullable: true
        }),
        chiefEngineer: new FormControl('', {
            nonNullable: true
        }),
        architects: new FormControl<string[]>([], {
            nonNullable: true
        }),
        engineers: new FormControl<string[]>([], {
            nonNullable: true
        }),
        bimSpecialists: new FormControl<string[]>([], {
            nonNullable: true
        }),
        visualizers: new FormControl<string[]>([], {
            nonNullable: true
        }),
        partners: new FormControl<string[]>([], {
            nonNullable: true
        }),
    });

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

        this.indicatorsForm.patchValue({
            totalArea: value.indicators.totalArea,
            plotArea: value.indicators.buildingArea,
            buildingArea: value.indicators.buildingArea,
            floorsCount: value.indicators.floorsCount,
            roomsCount: value.indicators.roomsCount,
            parkingSpacesCount: value.indicators.parkingSpacesCount,
            sectionsCount: value.indicators.sectionsCount
        }, { emitEvent: false });

        this.teamForm.patchValue({
            projectManager: value.projectManager,
            chiefEngineer: value.team.chiefArchitect,
            chiefArchitect: value.team.chiefArchitect,
            architects: value.team.architects,
            engineers: value.team.engineers,
            bimSpecialists: value.team.bimSpecialists,
            visualizers: value.team.visualizers,
            partners: value.team.partners
        }, { emitEvent: false });
    }

    public fromModel(status: ObjectStatus): Omit<IObject, 'id' | 'createdAt' | 'updatedAt'> {
        const mainFormValue = this.mainForm.getRawValue();
        const indicatorsFormValue = this.indicatorsForm.getRawValue();
        const teamFormValue = this.teamForm.getRawValue();

        return {
            title: mainFormValue.title,
            shortTitle: mainFormValue.shortTitle || undefined,
            city: mainFormValue.city,
            status,
            type: mainFormValue.type!,
            stage: mainFormValue.stage!,
            shortDescription: mainFormValue.shortDescription,
            customer: mainFormValue.customer || undefined,
            fullDescription: mainFormValue.fullDescription || undefined,
            projectManager: teamFormValue.projectManager,
            designYear: mainFormValue.designYear || undefined,
            implementationYear: mainFormValue.implementationYear || undefined,
            media: {

            } as unknown as IObjectMedia,
            indicators: {
                totalArea: indicatorsFormValue.totalArea!,
                plotArea: indicatorsFormValue.plotArea!,
                buildingArea: indicatorsFormValue.buildingArea!,
                sectionsCount: indicatorsFormValue.sectionsCount!,
                floorsCount: indicatorsFormValue.floorsCount ?? undefined,
                roomsCount: indicatorsFormValue.roomsCount ?? undefined,
                parkingSpacesCount: indicatorsFormValue.parkingSpacesCount ?? undefined
            },
            team: {
                chiefArchitect: teamFormValue.chiefArchitect || undefined,
                chiefEngineer: teamFormValue.chiefEngineer || undefined,
                architects: teamFormValue.architects.length ? teamFormValue.architects : undefined,
                engineers: teamFormValue.engineers.length ? teamFormValue.engineers : undefined,
                bimSpecialists: teamFormValue.bimSpecialists.length ? teamFormValue.bimSpecialists : undefined,
                visualizers: teamFormValue.visualizers.length ? teamFormValue.visualizers : undefined,
                partners: teamFormValue.partners.length ? teamFormValue.partners : undefined
            }
        };
    }

    public markInvalidAsTouched(): void {
        this._validationState.invalidForms.forEach((form) => form.markAllAsTouched());
    }
}
