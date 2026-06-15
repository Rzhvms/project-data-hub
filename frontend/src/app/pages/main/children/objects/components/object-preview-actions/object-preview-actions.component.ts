import { ChangeDetectionStrategy, Component, inject, input, InputSignal } from '@angular/core';
import { Router } from '@angular/router';
import { IObjectPreview } from '@project-data-hub/modules/objects';
import { AppRoute } from '@project-data-hub/shared';
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
    private readonly _router: Router = inject(Router);

    protected redirectToChild(childPath: string): void {
        this._router.navigate([
            AppRoute.ObjectsPage,
            childPath,
            this.object().id
        ]);
    }
}
