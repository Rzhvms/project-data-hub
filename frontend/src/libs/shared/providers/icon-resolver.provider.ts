import { Provider, SkipSelf } from '@angular/core';
import {type TuiStringHandler} from '@taiga-ui/cdk';
import { TUI_ICON_RESOLVER } from '@taiga-ui/core';

export const ICON_RESOLVER_PROVIDER: Provider = {
    provide: TUI_ICON_RESOLVER,
    deps: [[new SkipSelf(), TUI_ICON_RESOLVER]],
    useFactory(defaultResolver: TuiStringHandler<string>) {
        return (name: string): string =>
            name.startsWith('@tui.')
                ? defaultResolver(name)
                : `/icons/${name}.svg`;
    },
};
