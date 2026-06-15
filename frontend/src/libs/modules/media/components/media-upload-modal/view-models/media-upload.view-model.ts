import { FormControl, FormGroup, Validators } from '@angular/forms';

import { MediaImage } from '../../../types/media-image.type';

type MediaImageForm = {
    file: FormControl<File | null>;
    title: FormControl<string>;
    description: FormControl<string>;
    alternativeText: FormControl<string>;
};

export class MediaUploadViewModel {
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
    });

    private _idCounter: number = 0;

    public fromModel(): MediaImage {
        const formValue = this.form.getRawValue();

        return {
            id: `media-upload-${++this._idCounter}`,
            file: formValue.file!,
            title: formValue.title,
            description: formValue.description,
            alternativeText: formValue.alternativeText || undefined,
            useInSite: false,
            useInPresentation: false,
            useInPortfolio: false,
        };
    }
}
