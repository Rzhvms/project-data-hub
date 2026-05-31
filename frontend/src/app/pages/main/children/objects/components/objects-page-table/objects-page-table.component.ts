import { ChangeDetectionStrategy, Component, input, InputSignal } from '@angular/core';
import { IObjectPreview } from '@project-data-hub/modules/objects';
import { TuiTable } from '@taiga-ui/addon-table';
import { TuiButton, TuiDropdown } from '@taiga-ui/core';

@Component({
    selector: 'objects-page-table',
    templateUrl: './objects-page-table.component.html',
    styleUrl: './styles/objects-page-table.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiTable,
        TuiDropdown,
        TuiButton
    ]
})
export class ObjectsPageTableComponent {
    public readonly objectList: InputSignal<IObjectPreview[]> = input.required();
}
