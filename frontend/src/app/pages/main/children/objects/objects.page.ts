import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal, WritableSignal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { IObjectPreview, ObjectsRequestService } from '@project-data-hub/modules/objects';
import { AppRoute, IOption } from '@project-data-hub/shared';
import { TuiButton, TuiDropdown, TuiInput } from '@taiga-ui/core';
import { catchError, debounceTime, of, take } from 'rxjs';

import { ObjectsPageTableComponent } from './components/objects-page-table/objects-page-table.component';
import { ObjectsPageToolBarComponent } from './components/objects-page-tool-bar/objects-page-tool-bar.component';
import { ObjectsPageToolBarViewModel } from './components/objects-page-tool-bar/view-models/objects-page-tool-bar.view-model';

@Component({
    selector: 'objects-page',
    templateUrl: './objects.page.html',
    styleUrl: './styles/objects.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiButton,
        TuiInput,
        TuiDropdown,
        ObjectsPageToolBarComponent,
        ObjectsPageTableComponent
    ]
})
export class ObjectsPageComponent {
    protected readonly objectList: WritableSignal<IObjectPreview[]> = signal([]);
    protected readonly filteredObjectList: WritableSignal<IObjectPreview[]> = signal([]);
    protected readonly toolBarViewModel: ObjectsPageToolBarViewModel = new ObjectsPageToolBarViewModel();

    private readonly _router: Router = inject(Router);
    private readonly _requestService: ObjectsRequestService = inject(ObjectsRequestService);
    private readonly _destroyRef: DestroyRef = inject(DestroyRef);

    constructor() {
        this.setObjectList();
        this.initFilters();
        this.initChanges();
    }

    protected redirectToCreate(): void {
        this._router.navigate([
            AppRoute.ObjectsPage,
            'create'
        ]);
    }

    private setObjectList(): void {
        this._requestService.getObjectList()
            .pipe(
                take(1),
                catchError(() => of([]))
            )
            .subscribe((objectList) => {
                this.objectList.set([...objectList]);
                this.filteredObjectList.set([...objectList]);
            });
    }

    private initChanges(): void {
        this._requestService.changes$
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe(() => this.setObjectList());
    }

    private initFilters(): void {
        this.toolBarViewModel.form.valueChanges
            .pipe(
                debounceTime(300),
                takeUntilDestroyed(this._destroyRef)
            )
            .subscribe((filterValue) => {
                const statusFilters: Array<IOption<string>> = filterValue.statusFilter ?? [];
                const typeFilters: Array<IOption<string>> = filterValue.typeFilter ?? [];
                const searchValue: string = filterValue.search?.toLowerCase() ?? '';

                const result: IObjectPreview[] = this.objectList().filter((object) => {
                    const isStatusMatched: boolean =
                        statusFilters.length === 0 ||
                        statusFilters.some((filter) => filter.value === object.status);
                    const isTypeMatched: boolean =
                        typeFilters.length === 0 ||
                        typeFilters.some((filter) => filter.value === object.type);
                    const isSearchMatched: boolean =
                        !searchValue ||
                        object.title.toLowerCase().includes(searchValue) ||
                        object.city.toLowerCase().includes(searchValue) ||
                        new Date(object.createdAt).toLocaleDateString().includes(searchValue) ||
                        new Date(object.updatedAt).toLocaleDateString().includes(searchValue) ||
                        object.projectManager.toLowerCase().includes(searchValue);

                    return isStatusMatched && isTypeMatched && isSearchMatched;
                });

                this.filteredObjectList.set(result);
            });
    }
}
