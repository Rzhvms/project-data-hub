import { FormControl, FormGroup } from '@angular/forms';
import { OBJECT_STATUS_OPTIONS, OBJECT_TYPE_OPTIONS } from '@project-data-hub/modules/objects';
import { IOption, optionMatcher } from '@project-data-hub/shared';

type ToolBarForm = FormGroup<{
    search: FormControl<string>;
    statusFilter: FormControl<IOption[]>;
    typeFilter: FormControl<IOption[]>;
}>;

export class ObjectsPageToolBarViewModel {
    public readonly form: ToolBarForm = new FormGroup({
        search: new FormControl<string>('', {
            nonNullable: true,
        }),
        statusFilter: new FormControl<IOption[]>([], {
            nonNullable: true,
        }),
        typeFilter: new FormControl<IOption[]>([], {
            nonNullable: true,
        }),
    });

    public readonly statusFilterItems: IOption[] = OBJECT_STATUS_OPTIONS;
    public readonly typeFilterItems: IOption[] = OBJECT_TYPE_OPTIONS;
    public readonly filterItemMatcher = optionMatcher;
}
