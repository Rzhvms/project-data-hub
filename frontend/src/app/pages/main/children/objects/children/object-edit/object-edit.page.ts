import { ChangeDetectionStrategy, Component, inject, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IObject, IObjectFormValue, ObjectFormComponent } from '@project-data-hub/modules/objects';
import { AppRoute } from '@project-data-hub/shared';
import { TuiButton } from '@taiga-ui/core';
import { TuiButtonLoading } from '@taiga-ui/kit';
import { delay, switchMap, take, tap } from 'rxjs';

import { ObjectChildBasePageComponent } from '../base/object-child.base.page';

@Component({
    templateUrl: './object-edit.page.html',
    styleUrl: './styles/object-edit.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiButtonLoading,
        ObjectFormComponent
    ]
})
export class ObjectEditPageComponent extends ObjectChildBasePageComponent {
    private readonly _objectId: string | undefined = inject(ActivatedRoute).snapshot.params['objectId']

    protected readonly isSavingDraft: WritableSignal<boolean> = signal(false);
    protected readonly isPublishing: WritableSignal<boolean> = signal(false);

    constructor() {
        super();
        if (!this._objectId) {
            return;
        }

        this.requestService.getObjectById(this._objectId)
            .pipe(
                tap((value) => this.formViewModel.updateModel(value)),
                take(1)
            )
            .subscribe();
    }

    protected saveAsDraft(): void {
        const value: Partial<IObjectFormValue> = this.formViewModel.fromModelToDraft();
        this.isSavingDraft.set(true);

        this.requestService.updateObject(this._objectId!, value).pipe(
            delay(600),
            take(1),
            tap(() => this.isSavingDraft.set(false)),
        ).subscribe();
    }

    protected publish(): void {
        const invalidStep: number | null = this.formViewModel.validateAllAndGetFirstInvalidStep();
        if (invalidStep !== null) {
            this.formViewModel.activeStepIndex.set(invalidStep);

            return;
        }

        const value: IObjectFormValue = this.formViewModel.fromModel();
        this.isPublishing.set(true);

        this.requestService.updateObject(this._objectId!, value, 'published').pipe(
            delay(600),
            take(1),
            tap((obj: IObject | null) => {
                this.isPublishing.set(false);
                if (obj) {
                    this.router.navigate([AppRoute.ObjectsPage, 'view', obj.id]);
                }
            }),
        ).subscribe();
    }
}
