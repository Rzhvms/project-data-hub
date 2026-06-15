import { ChangeDetectionStrategy, Component, inject, OnDestroy, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
    getObjectUiStatus,
    IObject,
    OBJECT_STAGE_OPTIONS,
    OBJECT_STATUS_OPTIONS,
    OBJECT_TYPE_OPTIONS,
    ObjectStage,
    ObjectStatus,
    ObjectType,
} from '@project-data-hub/modules/objects';
import { AppRoute, createOptionLabelMap } from '@project-data-hub/shared';
import { TuiButton } from '@taiga-ui/core';
import { TuiAccordion, TuiBadge, TuiStatus, TuiTabs } from '@taiga-ui/kit';
import { take, tap } from 'rxjs';

import { ObjectChildBasePageComponent } from '../base/object-child.base.page';

@Component({
    templateUrl: './object-view.page.html',
    styleUrl: './styles/object-view.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiBadge,
        TuiStatus,
        TuiTabs,
        TuiAccordion,
    ]
})
export class ObjectViewPageComponent extends ObjectChildBasePageComponent implements OnDestroy {
    protected readonly object: WritableSignal<IObject | null> = signal(null);
    protected readonly activeTabIndex: WritableSignal<number> = signal(0);

    protected readonly objectTypeLabels: Record<ObjectType, string> = createOptionLabelMap(OBJECT_TYPE_OPTIONS);
    protected readonly objectStageLabels: Record<ObjectStage, string> = createOptionLabelMap(OBJECT_STAGE_OPTIONS);
    protected readonly objectStatusLabels: Record<ObjectStatus, string> = createOptionLabelMap(OBJECT_STATUS_OPTIONS);

    protected readonly getUiStatus = getObjectUiStatus;

    private readonly _objectId: string | undefined = inject(ActivatedRoute).snapshot.params['objectId'];
    private readonly _objectUrls: Map<string, string> = new Map<string, string>();

    constructor() {
        super();

        if (!this._objectId) {
            return;
        }

        this.requestService.getObjectById(this._objectId)
            .pipe(
                tap((value) => this.object.set(value)),
                take(1),
            )
            .subscribe();
    }

    public ngOnDestroy(): void {
        for (const url of this._objectUrls.values()) {
            URL.revokeObjectURL(url);
        }

        this._objectUrls.clear();
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

    protected formatValue(value: unknown): string {
        if (value === null || value === undefined) {
            return 'не заполнено';
        }

        if (typeof value === 'string') {
            return value || 'не заполнено';
        }

        if (typeof value === 'boolean') {
            return value ? 'Да' : 'Нет';
        }

        if (Array.isArray(value)) {
            return value.length ? value.join(', ') : 'не заполнено';
        }

        return String(value);
    }

    protected getImageUrl(file: File): string {
        const cached: string | undefined = this._objectUrls.get(file.name);

        if (cached) {
            return cached;
        }

        const url: string = URL.createObjectURL(file);
        this._objectUrls.set(file.name, url);

        return url;
    }

    protected trackById(index: number, item: { id: string }): string {
        return item.id;
    }
}
