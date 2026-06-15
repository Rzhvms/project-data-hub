import { ChangeDetectionStrategy, Component, input, InputSignal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TuiInput, TuiTextfield } from '@taiga-ui/core';
import { TuiFilter } from '@taiga-ui/kit';
import { TuiSearch } from '@taiga-ui/layout';

import { MediaPageToolBarViewModel } from './view-models/media-page-tool-bar.view-model';

@Component({
    selector: 'media-page-tool-bar',
    templateUrl: './media-page-tool-bar.component.html',
    styleUrl: './styles/media-page-tool-bar.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        ReactiveFormsModule,
        TuiTextfield,
        TuiFilter,
        TuiSearch,
        TuiInput,
    ],
})
export class MediaPageToolBarComponent {
    public readonly model: InputSignal<MediaPageToolBarViewModel> = input.required();
}
