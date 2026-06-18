import { ChangeDetectionStrategy, Component, signal, WritableSignal } from '@angular/core';
import { IObject, IObjectFormValue, ObjectFormComponent } from '@project-data-hub/modules/objects';
import { AppRoute } from '@project-data-hub/shared';
import { TuiButton } from '@taiga-ui/core';
import { TuiButtonLoading } from '@taiga-ui/kit';
import { delay, switchMap, take, tap } from 'rxjs';

import { ObjectChildBasePageComponent } from '../base/object-child.base.page';

@Component({
    templateUrl: './object-create.page.html',
    styleUrl: './styles/object-create.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiButtonLoading,
        ObjectFormComponent
    ]
})
export class ObjectCreatePageComponent extends ObjectChildBasePageComponent {
    protected readonly isSavingDraft: WritableSignal<boolean> = signal(false);
    protected readonly isPublishing: WritableSignal<boolean> = signal(false);

    protected saveAsDraft(): void {
        const value: Partial<IObjectFormValue> = this.formViewModel.fromModelToDraft();
        this.isSavingDraft.set(true);

        this.requestService.createObject(value as IObjectFormValue, 'draft').pipe(
            delay(600),
            take(1),
            tap((obj: IObject) => {
                this.isSavingDraft.set(false);
                this.router.navigate([AppRoute.ObjectsPage, 'view', obj.id]);
            }),
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

        this.requestService.createObject(value, 'published').pipe(
            delay(600),
            take(1),
            tap((obj: IObject) => {
                this.isPublishing.set(false);
                this.router.navigate([AppRoute.ObjectsPage, 'view', obj.id]);
            }),
        ).subscribe();
    }
}
