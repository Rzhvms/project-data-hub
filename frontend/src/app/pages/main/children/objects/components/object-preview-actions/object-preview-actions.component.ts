import { ChangeDetectionStrategy, Component, inject, input, InputSignal, signal, WritableSignal } from '@angular/core';
import { Router } from '@angular/router';
import { IObjectPreview, ObjectsRequestService } from '@project-data-hub/modules/objects';
import { AppRoute } from '@project-data-hub/shared';
import { TuiButton, TuiDropdown } from '@taiga-ui/core';
import { TuiButtonLoading } from '@taiga-ui/kit';
import { delay, take, tap } from 'rxjs';

@Component({
    selector: 'object-preview-actions',
    templateUrl: './object-preview-actions.component.html',
    styleUrl: './styles/object-preview-actions.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiDropdown,
        TuiButtonLoading,
    ]
})
export class ObjectPreviewActionsComponent {
    public readonly object: InputSignal<IObjectPreview> = input.required();

    protected readonly isExportingPptx: WritableSignal<boolean> = signal(false);
    protected readonly isExportingDocx: WritableSignal<boolean> = signal(false);

    private readonly _router: Router = inject(Router);
    private readonly _requestService: ObjectsRequestService = inject(ObjectsRequestService);

    protected redirectToChild(childPath: string): void {
        this._router.navigate([
            AppRoute.ObjectsPage,
            childPath,
            this.object().id
        ]);
    }

    protected exportTo(format: 'pptx' | 'docx'): void {
        const loading: WritableSignal<boolean> = format === 'pptx' ? this.isExportingPptx : this.isExportingDocx;

        loading.set(true);

        this._requestService.exportFile(this.object().id, format).pipe(
            delay(800),
            take(1),
            tap((blob: Blob) => {
                const url: string = URL.createObjectURL(blob);
                const anchor: HTMLAnchorElement = document.createElement('a');
                anchor.href = url;
                anchor.download = `${this.object().id}.${format}`;
                anchor.click();
                URL.revokeObjectURL(url);
            }),
        ).subscribe(() => loading.set(false));
    }
}
