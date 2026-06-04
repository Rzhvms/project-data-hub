import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'wp-settings-page',
    templateUrl: './wp-settings.page.html',
    styleUrl: './styles/wp-settings.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WPSettingsPageComponent {}
