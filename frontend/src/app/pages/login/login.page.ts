import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiIcon, TuiInput } from '@taiga-ui/core';
import { TuiPassword } from '@taiga-ui/kit';
import { AuthService } from '../../../libs/shared/auth';

@Component({
    selector: 'login-page',
    templateUrl: './login.page.html',
    styleUrl: './styles/login.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        ReactiveFormsModule,
        TuiInput,
        TuiIcon,
        TuiPassword,
        TuiButton
    ]
})
export class LoginPageComponent {
    protected readonly loginForm = new FormGroup({
        email: new FormControl(''),
        password: new FormControl('')
    })
    private readonly _authService: AuthService = inject(AuthService);

    protected login(): void {
        this._authService.authorize(this.loginForm.getRawValue())
    }
}
