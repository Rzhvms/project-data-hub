import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { MainPageAsideComponent } from './components/main-page-aside/main-page-aside.component';

@Component({
    selector: 'main-page',
    templateUrl: './main.page.html',
    styleUrl: './styles/main.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        MainPageAsideComponent,
        RouterOutlet
    ]
})
export class MainPageComponent {}
