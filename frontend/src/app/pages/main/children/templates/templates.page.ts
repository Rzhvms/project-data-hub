import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'templates-page',
    templateUrl: './templates.page.html',
    styleUrl: './styles/templates.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TemplatesPageComponent {}
