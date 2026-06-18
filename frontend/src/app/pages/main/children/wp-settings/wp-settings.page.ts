import { ChangeDetectionStrategy, Component, inject, signal, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {
    IConnectionTestResult,
    IWordpressSettings,
    WordpressRequestService,
} from '@project-data-hub/modules/wordpress';
import { TuiButton, TuiError, TuiInput, TuiTextfield } from '@taiga-ui/core';
import { TuiButtonLoading } from '@taiga-ui/kit';
import { take } from 'rxjs';

type WpSettingsForm = FormGroup<{
    siteUrl: FormControl<string>;
    username: FormControl<string>;
    applicationPassword: FormControl<string>;
}>;

@Component({
    templateUrl: './wp-settings.page.html',
    styleUrl: './styles/wp-settings.page.master.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    host: {
        style: 'display: block; height: 100%;',
    },
    imports: [ReactiveFormsModule, TuiButton, TuiButtonLoading, TuiInput, TuiTextfield, TuiError],
    providers: [WordpressRequestService],
})
export class WPSettingsPageComponent {
    protected readonly form: WpSettingsForm = new FormGroup({
        siteUrl: new FormControl<string>('', {
            nonNullable: true,
            validators: [Validators.required],
        }),
        username: new FormControl<string>('', {
            nonNullable: true,
            validators: [Validators.required],
        }),
        applicationPassword: new FormControl<string>('', {
            nonNullable: true,
            validators: [Validators.required],
        }),
    });

    protected readonly connectionStatus: WritableSignal<IConnectionTestResult | null> =
        signal(null);
    protected readonly connectedUrl: WritableSignal<string> = signal('');
    protected readonly isTesting: WritableSignal<boolean> = signal(false);
    protected readonly isConnected: WritableSignal<boolean> = signal(false);
    protected readonly isDisconnecting: WritableSignal<boolean> = signal(false);

    private readonly _requestService: WordpressRequestService = inject(WordpressRequestService);

    protected disconnect(): void {
        this.isDisconnecting.set(true);

        setTimeout(() => {
            this.isConnected.set(false);
            this.connectionStatus.set(null);
            this.connectedUrl.set('');
            this.form.reset();
            this.isDisconnecting.set(false);
        }, 800);
    }


    protected testConnection(): void {
        if (this.form.invalid) {
            this.form.markAllAsTouched();

            return;
        }

        this.isTesting.set(true);
        this.connectionStatus.set(null);

        const settings: IWordpressSettings = this.form.getRawValue();

        this.connectedUrl.set(settings.siteUrl);

        this._requestService
            .testConnection(settings)
            .pipe(take(1))
            .subscribe((result: IConnectionTestResult) => {
                this.isTesting.set(false);
                this.connectionStatus.set(result);

                if (result.success) {
                    this.isConnected.set(true);
                }
            });
    }
}
