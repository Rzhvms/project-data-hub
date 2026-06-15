import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiDialogContext, TuiError, TuiInput } from '@taiga-ui/core';
import { TuiFiles, TuiTextarea } from '@taiga-ui/kit';
import { injectContext } from '@taiga-ui/polymorpheus';

import { MediaImage } from '../../types/media-image.type';
import { MediaUploadViewModel } from './view-models/media-upload.view-model';

@Component({
    templateUrl: './media-upload-modal.component.html',
    styleUrl: './media-upload-modal.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        ReactiveFormsModule,
        TuiButton,
        TuiError,
        TuiFiles,
        TuiInput,
        TuiTextarea,
    ],
})
export class MediaUploadModalComponent {
    protected readonly context: TuiDialogContext<MediaImage, void> = injectContext();
    protected readonly model: MediaUploadViewModel = new MediaUploadViewModel();

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
