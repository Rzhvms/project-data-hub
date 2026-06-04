import { ChangeDetectionStrategy, Component, input, InputSignal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { getObjectUiStatus } from '@project-data-hub/modules/objects';
import { TuiAppearance,TuiButton, TuiDropdown, TuiInput, TuiTextfield } from '@taiga-ui/core';
import { TuiBadge, TuiChevron, TuiFilter, TuiStatus } from '@taiga-ui/kit';
import { TuiSearch } from '@taiga-ui/layout';

import { ObjectsPageToolBarViewModel } from './view-models/objects-page-tool-bar.view-model';

@Component({
    selector: 'objects-page-tool-bar',
    templateUrl: './objects-page-tool-bar.component.html',
    styleUrl: './styles/objects-page-tool-bar.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        ReactiveFormsModule,
        TuiTextfield,
        TuiChevron,
        TuiButton,
        TuiFilter,
        TuiDropdown,
        TuiSearch,
        TuiInput,
        TuiBadge,
        TuiStatus,
        TuiAppearance
    ]
})
export class ObjectsPageToolBarComponent {
    public readonly model: InputSignal<ObjectsPageToolBarViewModel> = input.required();
    protected getUiStatus = getObjectUiStatus;
}
