import { DatePipe } from '@angular/common';
import {
    afterNextRender,
    ChangeDetectionStrategy,
    Component,
    computed,
    DestroyRef,
    effect,
    ElementRef,
    inject,
    input,
    InputSignal,
    Signal,
    signal,
    viewChild,
    WritableSignal,
} from '@angular/core';
import {
    getObjectUiStatus,
    IObjectPreview,
    OBJECT_STATUS_OPTIONS,
    OBJECT_TYPE_OPTIONS,
    ObjectStatus,
    ObjectType,
} from '@project-data-hub/modules/objects';
import { createOptionLabelMap } from '@project-data-hub/shared';
import { TuiTable } from '@taiga-ui/addon-table';
import { TuiHintOverflow } from '@taiga-ui/core';
import { TuiBadge, TuiPagination, TuiStatus } from '@taiga-ui/kit';

import { ObjectPreviewActionsComponent } from '../object-preview-actions/object-preview-actions.component';

const PAGINATION_BLOCK_HEIGHT_PX = 64;

@Component({
    selector: 'objects-page-table',
    templateUrl: './objects-page-table.component.html',
    styleUrl: './styles/objects-page-table.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        TuiTable,
        TuiBadge,
        TuiStatus,
        TuiPagination,
        TuiHintOverflow,
        DatePipe,
        ObjectPreviewActionsComponent
    ],
})
export class ObjectsPageTableComponent {
    /** object lists */
    public readonly objectList: InputSignal<IObjectPreview[]> = input.required<IObjectPreview[]>();
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
    protected getUiStatus = getObjectUiStatus;

    /** pagination */
    protected readonly pagesCount: Signal<number> = computed(() =>
        Math.max(
            Math.ceil(this.objectList().length / this.rowsPerPageCount()),
            1,
        ),
    );
    protected readonly rowsPerPageCount: WritableSignal<number> = signal(1);
    protected readonly paginationIndex: WritableSignal<number> = signal(0);

    /** element refs */
    private readonly _tableWrapper: Signal<ElementRef<HTMLElement> | undefined> = viewChild<ElementRef<HTMLElement>>('tableWrapper')
    private readonly _tableHeader: Signal<ElementRef<HTMLElement> | undefined> = viewChild<ElementRef<HTMLElement>>('tableHeader');
    private readonly _tableRow: Signal<ElementRef<HTMLElement> | undefined> = viewChild<ElementRef<HTMLElement>>('tableRow');

    private _lastLayoutKey: string = '';
    private _resizeFrameId: number | null = null;

    private readonly _destroyRef: DestroyRef = inject(DestroyRef);

    constructor() {
        afterNextRender(() => this.initResizeObserver());
        effect(() => {
            if (this._tableRow()) {
                this.scheduleRowsPerPageUpdate();
            }
        });
        effect(() => this.clampPaginationIndex());

        this._destroyRef.onDestroy(() => this.cancelRowsPerPageUpdate());
    }

    private initResizeObserver(): void {
        const wrapperElement: HTMLElement | undefined = this._tableWrapper()?.nativeElement;
        if (!wrapperElement) {
            return;
        }

        const resizeObserver = new ResizeObserver(() => {
            this.scheduleRowsPerPageUpdate();
        });
        resizeObserver.observe(wrapperElement);

        this.scheduleRowsPerPageUpdate();
        this._destroyRef.onDestroy(() => resizeObserver.disconnect());
    }

    private scheduleRowsPerPageUpdate(): void {
        if (this._resizeFrameId !== null) {
            return;
        }

        this._resizeFrameId = requestAnimationFrame(() => {
            this._resizeFrameId = null;
            this.updateRowsPerPageCount();
        });
    }

    private cancelRowsPerPageUpdate(): void {
        if (this._resizeFrameId === null) {
            return;
        }

        cancelAnimationFrame(this._resizeFrameId);
        this._resizeFrameId = null;
    }

    private updateRowsPerPageCount(): void {
        const wrapperHeightPx: number = this._tableWrapper()?.nativeElement.clientHeight ?? 0;
        const headerHeightPx: number = this._tableHeader()?.nativeElement.offsetHeight ?? 0;
        const rowHeightPx: number = this._tableRow()?.nativeElement.offsetHeight ?? 0;
        if (!wrapperHeightPx || !headerHeightPx || !rowHeightPx) {
            return;
        }

        const layoutKey = `${wrapperHeightPx}:${headerHeightPx}:${rowHeightPx}`;
        if (layoutKey === this._lastLayoutKey) {
            return;
        }
        this._lastLayoutKey = layoutKey;

        const availableTableHeightPx: number = wrapperHeightPx - PAGINATION_BLOCK_HEIGHT_PX - headerHeightPx;
        const rowsPerPageCount: number = Math.max(
            Math.floor(availableTableHeightPx / rowHeightPx),
            1,
        );
        if (rowsPerPageCount !== this.rowsPerPageCount()) {
            this.rowsPerPageCount.set(rowsPerPageCount);
        }
    }

    private clampPaginationIndex(): void {
        const maxIndex: number = this.pagesCount() - 1;
        if (this.paginationIndex() > maxIndex) {
            this.paginationIndex.set(maxIndex);
        }
    }
}
