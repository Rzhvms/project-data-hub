import { signal, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MediaImage } from '@project-data-hub/modules/media';
import { IObject,IObjectFormValue,OBJECT_STAGE_OPTIONS, OBJECT_TYPE_OPTIONS, ObjectStage, ObjectType } from '@project-data-hub/modules/objects';
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

    public readonly activeStepIndex: WritableSignal<number> = signal(0);

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

    public fromModel(): IObjectFormValue {
        const mainFormValue = this.mainForm.getRawValue();
        const mediaFormValue = this.mediaForm.getRawValue();
        const indicatorsFormValue = this.indicatorsForm.getRawValue();
        const teamFormValue = this.teamForm.getRawValue();

        return {
            title: mainFormValue.title,
            city: mainFormValue.city,
            type: mainFormValue.type!,
            stage: mainFormValue.stage!,
            shortDescription: mainFormValue.shortDescription,
            projectManager: teamFormValue.projectManager,
            ...(mainFormValue.shortTitle && { shortTitle: mainFormValue.shortTitle }),
            ...(mainFormValue.customer && { customer: mainFormValue.customer }),
            ...(mainFormValue.fullDescription && { fullDescription: mainFormValue.fullDescription }),
            ...(mainFormValue.designYear !== null && { designYear: mainFormValue.designYear }),
            ...(mainFormValue.implementationYear !== null && { implementationYear: mainFormValue.implementationYear }),
            media: {
                ...(mediaFormValue.mainImage && { mainImage: mediaFormValue.mainImage }),
                ...(mediaFormValue.presentationCover && { presentationCover: mediaFormValue.presentationCover }),
                ...(mediaFormValue.images.length && { images: mediaFormValue.images }),
                ...(mediaFormValue.schemas.length && { schemas: mediaFormValue.schemas }),
                ...(mediaFormValue.renders.length && { renders: mediaFormValue.renders }),
                ...(mediaFormValue.photos.length && { photos: mediaFormValue.photos }),
                ...(mediaFormValue.portfolioImages.length && { portfolioImages: mediaFormValue.portfolioImages }),
            },
            indicators: {
                ...(indicatorsFormValue.totalArea !== null && { totalArea: indicatorsFormValue.totalArea }),
                ...(indicatorsFormValue.plotArea !== null && { plotArea: indicatorsFormValue.plotArea }),
                ...(indicatorsFormValue.buildingArea !== null && { buildingArea: indicatorsFormValue.buildingArea }),
                ...(indicatorsFormValue.sectionsCount !== null && { sectionsCount: indicatorsFormValue.sectionsCount }),
                ...(indicatorsFormValue.floorsCount !== null && { floorsCount: indicatorsFormValue.floorsCount }),
                ...(indicatorsFormValue.roomsCount !== null && { roomsCount: indicatorsFormValue.roomsCount }),
                ...(indicatorsFormValue.parkingSpacesCount !== null && { parkingSpacesCount: indicatorsFormValue.parkingSpacesCount }),
            },
            team: {
                ...(teamFormValue.chiefArchitect && { chiefArchitect: teamFormValue.chiefArchitect }),
                ...(teamFormValue.chiefEngineer && { chiefEngineer: teamFormValue.chiefEngineer }),
                ...(teamFormValue.architects.length && { architects: teamFormValue.architects }),
                ...(teamFormValue.engineers.length && { engineers: teamFormValue.engineers }),
                ...(teamFormValue.bimSpecialists.length && { bimSpecialists: teamFormValue.bimSpecialists }),
                ...(teamFormValue.visualizers.length && { visualizers: teamFormValue.visualizers }),
                ...(teamFormValue.partners.length && { partners: teamFormValue.partners }),
            },
        } as IObjectFormValue;
    }

    public fromModelToDraft(): Partial<IObjectFormValue> {
        const mainFormValue = this.mainForm.getRawValue();
        const mediaFormValue = this.mediaForm.getRawValue();
        const indicatorsFormValue = this.indicatorsForm.getRawValue();
        const teamFormValue = this.teamForm.getRawValue();

        return {
            ...(mainFormValue.title && { title: mainFormValue.title }),
            ...(mainFormValue.shortTitle && { shortTitle: mainFormValue.shortTitle }),
            ...(mainFormValue.city && { city: mainFormValue.city }),
            ...(mainFormValue.type !== null && { type: mainFormValue.type }),
            ...(mainFormValue.stage !== null && { stage: mainFormValue.stage }),
            ...(mainFormValue.shortDescription && { shortDescription: mainFormValue.shortDescription }),
            ...(mainFormValue.customer && { customer: mainFormValue.customer }),
            ...(mainFormValue.fullDescription && { fullDescription: mainFormValue.fullDescription }),
            ...(teamFormValue.projectManager && { projectManager: teamFormValue.projectManager }),
            ...(mainFormValue.designYear !== null && { designYear: mainFormValue.designYear }),
            ...(mainFormValue.implementationYear !== null && { implementationYear: mainFormValue.implementationYear }),
            media: {
                ...(mediaFormValue.mainImage && { mainImage: mediaFormValue.mainImage }),
                ...(mediaFormValue.presentationCover && { presentationCover: mediaFormValue.presentationCover }),
                ...(mediaFormValue.images.length && { images: mediaFormValue.images }),
                ...(mediaFormValue.schemas.length && { schemas: mediaFormValue.schemas }),
                ...(mediaFormValue.renders.length && { renders: mediaFormValue.renders }),
                ...(mediaFormValue.photos.length && { photos: mediaFormValue.photos }),
                ...(mediaFormValue.portfolioImages.length && { portfolioImages: mediaFormValue.portfolioImages }),
            },
            indicators: {
                ...(indicatorsFormValue.totalArea !== null && { totalArea: indicatorsFormValue.totalArea }),
                ...(indicatorsFormValue.plotArea !== null && { plotArea: indicatorsFormValue.plotArea }),
                ...(indicatorsFormValue.buildingArea !== null && { buildingArea: indicatorsFormValue.buildingArea }),
                ...(indicatorsFormValue.sectionsCount !== null && { sectionsCount: indicatorsFormValue.sectionsCount }),
                ...(indicatorsFormValue.floorsCount !== null && { floorsCount: indicatorsFormValue.floorsCount }),
                ...(indicatorsFormValue.roomsCount !== null && { roomsCount: indicatorsFormValue.roomsCount }),
                ...(indicatorsFormValue.parkingSpacesCount !== null && { parkingSpacesCount: indicatorsFormValue.parkingSpacesCount }),
            },
            team: {
                ...(teamFormValue.chiefArchitect && { chiefArchitect: teamFormValue.chiefArchitect }),
                ...(teamFormValue.chiefEngineer && { chiefEngineer: teamFormValue.chiefEngineer }),
                ...(teamFormValue.architects.length && { architects: teamFormValue.architects }),
                ...(teamFormValue.engineers.length && { engineers: teamFormValue.engineers }),
                ...(teamFormValue.bimSpecialists.length && { bimSpecialists: teamFormValue.bimSpecialists }),
                ...(teamFormValue.visualizers.length && { visualizers: teamFormValue.visualizers }),
                ...(teamFormValue.partners.length && { partners: teamFormValue.partners }),
            },
        } as Partial<IObjectFormValue>;
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
