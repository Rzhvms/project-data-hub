import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TuiAppearance, TuiButton, TuiError,TuiInput } from '@taiga-ui/core';
import { TuiFiles, TuiSwitch, TuiTextarea } from '@taiga-ui/kit';
import { injectContext } from '@taiga-ui/polymorpheus';

import { ObjectImageModalContext } from '../../types/object-image-modal-context.type';
import { ObjectImageViewModel } from './view-models/object-image.view-model';

@Component({
    templateUrl: './object-image-modal.component.html',
    styleUrl: './styles/object-image-modal.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        ReactiveFormsModule,
        TuiButton,
        TuiInput,
        TuiTextarea,
        TuiSwitch,
        TuiAppearance,
        TuiFiles,
        TuiError,
    ],
})
export class ObjectImageModalComponent {
    protected readonly context: ObjectImageModalContext = injectContext<ObjectImageModalContext>();
    protected readonly model: ObjectImageViewModel = new ObjectImageViewModel(
        this.context.data ?? undefined,
    );

    protected removeFile(): void {
        this.model.form.controls.file.setValue(null);
    }

    protected save(): void {
        if (!this.model.form.valid) {
            this.model.form.markAllAsTouched();
        } else {
            this.context.completeWith(this.model.fromModel());
        }
    }
}
