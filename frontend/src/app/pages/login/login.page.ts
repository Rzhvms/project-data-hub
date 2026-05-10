import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TuiButton, TuiError, TuiIcon, TuiInput } from '@taiga-ui/core';
import { TuiButtonLoading, TuiPassword } from '@taiga-ui/kit';
import { catchError, debounceTime, finalize, take, throwError } from 'rxjs';

import { AuthService } from '../../../libs/shared/auth';
import { ApiErrorCode } from '../../../libs/shared/enums';

type LoginForm = FormGroup<{
    email: FormControl<string>;
    password: FormControl<string>;
}>;

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
        TuiButton,
        TuiButtonLoading,
        TuiError,
    ]
})
export class LoginPageComponent {
    protected readonly isLoading = signal(false);
    protected readonly authError = signal<string | null>(null);
    protected readonly loginForm: LoginForm = new FormGroup({
        email: new FormControl('', {
            nonNullable: true,
            validators: [
                Validators.required,
                Validators.email
            ]
        }),
        password: new FormControl('', {
            nonNullable: true,
            validators: [Validators.required]
        })
    })

    private readonly _authService: AuthService = inject(AuthService);
    private readonly _destroyRef: DestroyRef = inject(DestroyRef);

    constructor() {
        this.handleFormValueChanges();
    }

    protected login(): void {
        if (this.loginForm.invalid) {
            this.loginForm.markAllAsTouched();

            return;
        }

        this.isLoading.set(true);

        this._authService.authorize(this.loginForm.getRawValue())
            .pipe(
                take(1),
                catchError((error: HttpErrorResponse) => {
                    const errorCode = error?.error?.code;
                    if (
                        errorCode === ApiErrorCode.CredentialsAreNotValid ||
                        errorCode === ApiErrorCode.UserDoesNotExist
                    ) {
                        this.authError.set('Неверный email или пароль');
                    } else {
                        this.authError.set('Произошла ошибка, попробуйте позже');
                    }

                    return throwError(() => error);
                }),
                finalize(() => this.isLoading.set(false))
            )
            .subscribe();
    }

    private handleFormValueChanges(): void {
        this.loginForm.valueChanges
            .pipe(
                debounceTime(200),
                takeUntilDestroyed(this._destroyRef)
            )
            .subscribe(() => this.authError.set(null))
    }
}
