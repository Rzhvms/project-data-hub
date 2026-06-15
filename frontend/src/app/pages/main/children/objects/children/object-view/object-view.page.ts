import { ChangeDetectionStrategy, Component, inject, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { getObjectUiStatus, IObject, OBJECT_STATUS_OPTIONS, ObjectStatus } from '@project-data-hub/modules/objects';
import { AppRoute, createOptionLabelMap } from '@project-data-hub/shared';
import { TuiButton } from '@taiga-ui/core';
import { TuiBadge, TuiStatus } from '@taiga-ui/kit';
import { take, tap } from 'rxjs';

import { ObjectChildBasePageComponent } from '../base/object-child.base.page';

@Component({
    templateUrl: './object-view.page.html',
    styleUrl: './styles/object-view.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiBadge,
        TuiStatus
    ]
})
export class ObjectViewPageComponent extends ObjectChildBasePageComponent {
    protected readonly object: WritableSignal<IObject | null> = signal(null);

    protected readonly objectStatusLabels: Record<ObjectStatus, string> = createOptionLabelMap(OBJECT_STATUS_OPTIONS);
    protected getUiStatus = getObjectUiStatus;

    private readonly _objectId: string | undefined = inject(ActivatedRoute).snapshot.params['objectId']

    constructor() {
        super();
        if (!this._objectId) {
            return;
        }

        this.requestService.getObjectById(this._objectId)
            .pipe(
                tap((value) => this.object.set(value)),
                take(1)
            )
            .subscribe();
    }

    protected edit(): void {
        this.router.navigate([AppRoute.ObjectsPage, 'edit', this._objectId]);
    }

    protected archivate(): void {
        console.log(this._objectId);
    }

    protected returnFromArchive(): void {
        console.log(this._objectId);
    }
}
