import { ChangeDetectionStrategy, Component } from '@angular/core';

import { ObjectChildBasePageComponent } from '../base/object-child.base.page';

@Component({
    templateUrl: './object-view.page.html',
    styleUrl: './styles/object-view.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ObjectViewPageComponent extends ObjectChildBasePageComponent {
}
