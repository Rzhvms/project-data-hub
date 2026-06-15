import { FormControl, FormGroup } from '@angular/forms';

type ToolBarForm = FormGroup<{
    search: FormControl<string>;
    usageFilter: FormControl<string[]>;
}>;

export class MediaPageToolBarViewModel {
    public readonly form: ToolBarForm = new FormGroup({
        search: new FormControl<string>('', {
            nonNullable: true,
        }),
        usageFilter: new FormControl<string[]>([], {
            nonNullable: true,
        }),
    });

    public readonly usageFilterItems: string[] = [
        'На сайте',
        'В презентации',
        'В портфолио',
    ];
}
