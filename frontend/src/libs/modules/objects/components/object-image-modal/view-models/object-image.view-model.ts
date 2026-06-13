import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MediaImage } from '@project-data-hub/modules/media';

type MediaImageForm = {
    file: FormControl<File | null>;
    title: FormControl<string>;
    description: FormControl<string>;
    alternativeText: FormControl<string>;
    useInSite: FormControl<boolean>;
    useInPresentation: FormControl<boolean>;
    useInPortfolio: FormControl<boolean>;
};

export class ObjectImageViewModel {
    public readonly form: FormGroup<MediaImageForm> = new FormGroup<MediaImageForm>({
        file: new FormControl<File | null>(null, {
            validators: [Validators.required]
        }),
        title: new FormControl<string>('', {
            nonNullable: true,
            validators: [Validators.required]
        }),
        description: new FormControl<string>('', {
            nonNullable: true,
            validators: [Validators.required]
        }),
        alternativeText: new FormControl<string>('', {
            nonNullable: true,
        }),
        useInSite: new FormControl<boolean>(false, {
            nonNullable: true
        }),
        useInPresentation: new FormControl<boolean>(false, {
            nonNullable: true
        }),
        useInPortfolio: new FormControl<boolean>(false, {
            nonNullable: true
        }),
    });

    private readonly _id: string = '';

    constructor(defaultValue?: MediaImage) {
        if (defaultValue) {
            this.form.setValue({
                file: defaultValue.file,
                title: defaultValue.title,
                description: defaultValue.description,
                alternativeText: defaultValue.alternativeText ?? '',
                useInPortfolio: defaultValue.useInPortfolio,
                useInPresentation: defaultValue.useInPresentation,
                useInSite: defaultValue.useInSite
            }, { emitEvent: false });

            this._id = defaultValue.id;
        }
    }

    public fromModel(): MediaImage {
        const formValue = this.form.getRawValue();

        return {
            id: this._id,
            file: formValue.file!,
            title: formValue.title,
            description: formValue.description,
            alternativeText: formValue.alternativeText || undefined,
            useInSite: formValue.useInSite,
            useInPresentation: formValue.useInPresentation,
            useInPortfolio: formValue.useInPortfolio
        };
    }
}
