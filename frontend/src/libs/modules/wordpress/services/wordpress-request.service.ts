import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

import { IConnectionTestResult, ISyncStatus, IWordpressSettings } from '../types/wordpress-connection.type';

const LS_KEY = 'data-hub-mock:wordpress-settings';
const LS_CONNECTION_KEY = 'data-hub-mock:wordpress-connection';

interface PersistedConnection {
    isConnected: boolean;
    connectionStatus: IConnectionTestResult;
    connectedUrl: string;
}

@Injectable()
export class WordpressRequestService {
    private _settings: IWordpressSettings | null = null;
    private _connectionStatus: IConnectionTestResult | null = null;
    private _syncStatus: ISyncStatus = {
        lastSync: null,
        postsCount: 0,
        pagesCount: 0,
        mediaCount: 0,
        isSyncing: false,
    };

    constructor() {
        const saved: string | null = localStorage.getItem(LS_KEY);

        if (saved) {
            try {
                this._settings = JSON.parse(saved) as IWordpressSettings;
            } catch {
                /* ignore */
            }
        }
    }

    public getSettings(): IWordpressSettings | null {
        return this._settings;
    }

    public getPersistedConnection(): PersistedConnection | null {
        const raw = localStorage.getItem(LS_CONNECTION_KEY);

        if (raw) {
            try {
                return JSON.parse(raw) as PersistedConnection;
            } catch {
                /* ignore */
            }
        }

        return null;
    }

    public persistConnection(state: PersistedConnection): void {
        localStorage.setItem(LS_CONNECTION_KEY, JSON.stringify(state));
    }

    public clearPersistedConnection(): void {
        localStorage.removeItem(LS_CONNECTION_KEY);
    }

    public testConnection(settings: IWordpressSettings): Observable<IConnectionTestResult> {
        this._settings = settings;
        localStorage.setItem(LS_KEY, JSON.stringify(settings));

        const result: IConnectionTestResult = {
            success: true,
            message: 'Подключение установлено',
            siteName: 'Мой сайт на WordPress',
            wpVersion: '6.7.2',
            userId: 1,
            userDisplayName: settings.username,
        };

        this._connectionStatus = result;

        return of(result).pipe(delay(1200));
    }

    public getSyncStatus(): Observable<ISyncStatus> {
        return of(this._syncStatus).pipe(delay(300));
    }

    public startSync(): Observable<ISyncStatus> {
        this._syncStatus = {
            ...this._syncStatus,
            isSyncing: true,
        };

        const completed: ISyncStatus = {
            lastSync: new Date().toISOString(),
            postsCount: 15,
            pagesCount: 6,
            mediaCount: 43,
            isSyncing: false,
        };

        return of(completed).pipe(delay(2500));
    }

    public getConnectionStatus(): IConnectionTestResult | null {
        return this._connectionStatus;
    }
}
