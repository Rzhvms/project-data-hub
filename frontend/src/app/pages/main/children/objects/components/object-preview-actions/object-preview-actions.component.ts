import { ChangeDetectionStrategy, Component, input, InputSignal } from '@angular/core';
import { IObjectPreview } from '@project-data-hub/modules/objects';
import { TuiButton, TuiDropdown } from '@taiga-ui/core';

@Component({
    selector: 'object-preview-actions',
    templateUrl: './object-preview-actions.component.html',
    styleUrl: './styles/object-preview-actions.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiDropdown
    ]
})
export class ObjectPreviewActionsComponent {
    public readonly object: InputSignal<IObjectPreview> = input.required();
}
