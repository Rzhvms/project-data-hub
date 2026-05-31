import { ChangeDetectionStrategy, Component, input, InputSignal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiDropdown, TuiInput, TuiTextfield } from '@taiga-ui/core';
import { TuiChevron, TuiFilter } from '@taiga-ui/kit';
import { TuiSearch } from '@taiga-ui/layout';

import { ObjectsPageToolBarViewModel } from './view-models/objects-page-tool-bar.view-model';

@Component({
    selector: 'objects-page-tool-bar',
    templateUrl: './objects-page-tool-bar.component.html',
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
    ]
})
export class ObjectsPageToolBarComponent {
    public readonly model: InputSignal<ObjectsPageToolBarViewModel> = input.required();
}
