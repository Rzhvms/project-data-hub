import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MediaImage } from '@project-data-hub/modules/media';
import { IObject,OBJECT_STAGE_OPTIONS, OBJECT_TYPE_OPTIONS, ObjectStage, ObjectStatus, ObjectType } from '@project-data-hub/modules/objects';
import { IOption } from '@project-data-hub/shared';

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

type MediaForm = {
    mainImage: FormControl<MediaImage | null>;
    presentationCover: FormControl<MediaImage | null>;
    images: FormControl<MediaImage[]>;
    schemas: FormControl<MediaImage[]>;
    renders: FormControl<MediaImage[]>;
    photos: FormControl<MediaImage[]>;
    portfolioImages: FormControl<MediaImage[]>;
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
    public readonly mediaForm: FormGroup<MediaForm> = new FormGroup<MediaForm>({
        mainImage: new FormControl<MediaImage | null>(null, {
            validators: [Validators.required]
        }),
        presentationCover: new FormControl<MediaImage | null>(null),
        images: new FormControl<MediaImage[]>([], {
            nonNullable: true
        }),
        schemas: new FormControl<MediaImage[]>([], {
            nonNullable: true
        }),
        renders: new FormControl<MediaImage[]>([], {
            nonNullable: true
        }),
        photos: new FormControl<MediaImage[]>([], {
            nonNullable: true
        }),
        portfolioImages: new FormControl<MediaImage[]>([], {
            nonNullable: true
        })
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

    private readonly _forms: FormGroup[] = [
        this.mainForm,
        this.mediaForm,
        this.indicatorsForm,
        this.teamForm
    ];

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

        this.mediaForm.patchValue({
            mainImage: value.media.mainImage,
            images: value.media.images,
            schemas: value.media.schemas,
            renders: value.media.renders,
            photos: value.media.photos,
            presentationCover: value.media.presentationCover,
            portfolioImages: value.media.portfolioImages
        }, { emitEvent: false });

        this.indicatorsForm.patchValue({
            totalArea: value.indicators.totalArea,
            plotArea: value.indicators.plotArea,
            buildingArea: value.indicators.buildingArea,
            floorsCount: value.indicators.floorsCount,
            roomsCount: value.indicators.roomsCount,
            parkingSpacesCount: value.indicators.parkingSpacesCount,
            sectionsCount: value.indicators.sectionsCount
        }, { emitEvent: false });

        this.teamForm.patchValue({
            projectManager: value.projectManager,
            chiefEngineer: value.team.chiefEngineer,
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
        const mediaFormValue = this.mediaForm.getRawValue();
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
                mainImage: mediaFormValue.mainImage!,
                images: mediaFormValue.images.length ? mediaFormValue.images : undefined,
                photos: mediaFormValue.photos.length ? mediaFormValue.photos : undefined,
                portfolioImages: mediaFormValue.portfolioImages.length ? mediaFormValue.portfolioImages : undefined,
                presentationCover: mediaFormValue.presentationCover ?? undefined,
                renders: mediaFormValue.renders.length ? mediaFormValue.renders : undefined,
                schemas: mediaFormValue.schemas.length ? mediaFormValue.schemas : undefined
            },
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

    public isStepValid(stepIndex: number): boolean {
        return this._forms[stepIndex]?.valid ?? true;
    }

        public markStepAsTouched(stepIndex: number): void {
        this._forms[stepIndex]?.markAllAsTouched();
    }

    public validateAllAndGetFirstInvalidStep(): number | null {
        for (let i = 0; i < this._forms.length; i++) {
            if (this._forms[i].invalid) {
                this._forms[i].markAllAsTouched();

                return i;
            }
        }

        return null;
    }
}
