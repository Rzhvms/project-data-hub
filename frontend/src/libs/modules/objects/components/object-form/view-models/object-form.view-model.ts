import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ObjectStage } from '@project-data-hub/modules/objects/constants/object-stage.constant';
import { ObjectStatus } from '@project-data-hub/modules/objects/constants/object-status.constant';
import { ObjectType } from '@project-data-hub/modules/objects/constants/object-type.constant';

export class ObjectFormViewModel {
    public readonly stepList: string[] = [
        'Основные',
        'Медиа',
        'Размеры',
        'Команда'
    ];

    public readonly mainForm = new FormGroup({
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
        fullDescription: new FormControl('', {
            nonNullable: true,
        }),
        shortDescription: new FormControl('', {
            nonNullable: true
        }),
        status: new FormControl<ObjectStatus | null>(null, {
            validators: [Validators.required]
        }),
        stage: new FormControl<ObjectStage | null>(null, {
            validators: [Validators.required]
        }),
        designYear: new FormControl('', {
            nonNullable: true
        }),
        implementationYear: new FormControl('', {
            nonNullable: true
        })
    });
}
