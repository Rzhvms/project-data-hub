import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import {
    OBJECT_STATUS_OPTIONS,
    OBJECT_TYPE_OPTIONS,
    ObjectStatus,
    ObjectType,
} from '@project-data-hub/modules/object';
import { optionMatcher } from '@project-data-hub/shared/utils';
import { TuiTable } from '@taiga-ui/addon-table';
import { TuiButton, TuiDropdown, TuiInput } from '@taiga-ui/core';
import { TuiChevron, TuiFilter } from '@taiga-ui/kit';
import { TuiSearch } from '@taiga-ui/layout';

type ToolBarForm = FormGroup<{
    search: FormControl<string>;
    statusFilter: FormControl<ObjectStatus[]>;
    typeFilter: FormControl<ObjectType[]>;
}>;

@Component({
    selector: 'objects-page',
    templateUrl: './objects.page.html',
    styleUrl: './styles/objects.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        ReactiveFormsModule,
        TuiButton,
        TuiSearch,
        TuiInput,
        TuiChevron,
        TuiDropdown,
        TuiFilter,
        TuiTable,
    ],
})
export class ObjectsPageComponent {
    protected readonly toolBarForm: ToolBarForm = new FormGroup({
        search: new FormControl<string>('', {
            nonNullable: true,
        }),
        statusFilter: new FormControl<ObjectStatus[]>([], {
            nonNullable: true,
        }),
        typeFilter: new FormControl<ObjectType[]>([], {
            nonNullable: true,
        }),
    });

    protected readonly statusFilterItems: typeof OBJECT_STATUS_OPTIONS = OBJECT_STATUS_OPTIONS;
    protected readonly typeFilterItems: typeof OBJECT_TYPE_OPTIONS = OBJECT_TYPE_OPTIONS;
    protected readonly filterItemMatcher = optionMatcher;
}
