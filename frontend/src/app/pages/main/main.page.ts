import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
    selector: 'main-page',
    templateUrl: './main.page.html',
    styleUrl: './styles/main.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainPageComponent {

}
