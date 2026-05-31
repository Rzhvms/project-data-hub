import { DatePipe } from '@angular/common';
import { afterNextRender, ChangeDetectionStrategy, Component, computed, DestroyRef, effect, ElementRef, inject, input, InputSignal, Signal, signal, viewChild, WritableSignal } from '@angular/core';
import { IObjectPreview, OBJECT_STATUS_OPTIONS, OBJECT_TYPE_OPTIONS, ObjectStatus, ObjectType } from '@project-data-hub/modules/objects';
import { createOptionLabelMap } from '@project-data-hub/shared';
import { TuiTable } from '@taiga-ui/addon-table';
import { TuiButton, TuiDropdown } from '@taiga-ui/core';
import { TuiPagination } from '@taiga-ui/kit';

type HeightMeasurments = {
    wrapperHeightPx: number,
    headerHeightPx: number,
    rowHeightPx: number
}

const PAGINATION_BLOCK_HEIGHT_PX = 64;

@Component({
    selector: 'objects-page-table',
    templateUrl: './objects-page-table.component.html',
    styleUrl: './styles/objects-page-table.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiTable,
        TuiPagination,
        TuiDropdown,
        TuiButton,
        DatePipe,
    ]
})
export class ObjectsPageTableComponent {
    /** object lists */
    public readonly objectList: InputSignal<IObjectPreview[]> = input.required();
    protected readonly paginatedObjectList: Signal<IObjectPreview[]> = computed(() => {
        const startIndex: number = this.paginationIndex() * this.rowsPerPageCount();

        return this.objectList().slice(
            startIndex,
            startIndex + this.rowsPerPageCount()
        );
    });

    /** labels */
    protected readonly objectTypeLabels: Record<ObjectType, string> = createOptionLabelMap(OBJECT_TYPE_OPTIONS);
    protected readonly objectStatusLabels: Record<ObjectStatus, string> = createOptionLabelMap(OBJECT_STATUS_OPTIONS);

    /** pagination */
    protected readonly pagesCount: Signal<number> = computed(() => Math.ceil(
        Math.max(
            this.objectList().length / this.rowsPerPageCount(),
            1
        )
    ));
    protected readonly rowsPerPageCount: WritableSignal<number> = signal(1);
    protected readonly paginationIndex: WritableSignal<number> = signal(0);

    /** element refs */
    private readonly _tableWrapper: Signal<ElementRef<HTMLElement> | undefined> = viewChild('tableWrapper', { read: ElementRef });
    private readonly _tableHeader: Signal<ElementRef<HTMLElement> | undefined> = viewChild('tableHeader', { read: ElementRef });
    private readonly _tableRow: Signal<ElementRef<HTMLElement> | undefined> = viewChild('tableRow', { read: ElementRef });

    /** last height values */
    private _lastMeasurments: HeightMeasurments = {
        wrapperHeightPx: 0,
        headerHeightPx: 0,
        rowHeightPx: 0
    }

    private readonly _destroyRef: DestroyRef = inject(DestroyRef);

    constructor() {
        afterNextRender(() => this.initResizeObserver());
        effect(() => {
            const maxIndex = Math.max(this.pagesCount() - 1, 0);
            if (this.paginationIndex() > maxIndex) {
                this.paginationIndex.set(maxIndex);
            }
        });
    }

    private initResizeObserver(): void {
        const wrapperElement: HTMLElement | undefined = this._tableWrapper()?.nativeElement;
        if (!wrapperElement) {
            return;
        }

        const resizeObserver = new ResizeObserver(() => this.updateRowsPerPageCount());
        resizeObserver.observe(wrapperElement);

        requestAnimationFrame(() => this.updateRowsPerPageCount());
        this._destroyRef.onDestroy(() => resizeObserver.disconnect());
    }

    private updateRowsPerPageCount(): void {
        const wrapperHeightPx = this._tableWrapper()?.nativeElement.clientHeight;
        const headerHeightPx = this._tableHeader()?.nativeElement.offsetHeight;
        const rowHeightPx = this._tableRow()?.nativeElement.offsetHeight;

        if (!wrapperHeightPx || !headerHeightPx || !rowHeightPx) {
            return;
        }
        if (
            wrapperHeightPx === this._lastMeasurments.wrapperHeightPx &&
            headerHeightPx === this._lastMeasurments.headerHeightPx &&
            rowHeightPx === this._lastMeasurments.rowHeightPx
        ) {
            return;
        }

        this._lastMeasurments = {
            wrapperHeightPx,
            headerHeightPx,
            rowHeightPx
        };

        const rowsCount = Math.max(
            Math.floor(
                (wrapperHeightPx - PAGINATION_BLOCK_HEIGHT_PX - headerHeightPx) / rowHeightPx
            ),
            1
        );
        if (rowsCount !== this.rowsPerPageCount()) {
            this.rowsPerPageCount.set(rowsCount);
        }
    }
}
