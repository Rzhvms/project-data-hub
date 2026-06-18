import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

import { IConnectionTestResult, ISyncStatus, IWordpressSettings } from '../types/wordpress-connection.type';

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

    public getSettings(): IWordpressSettings | null {
        return this._settings;
    }

    public testConnection(settings: IWordpressSettings): Observable<IConnectionTestResult> {
        this._settings = settings;

        const result: IConnectionTestResult = {
            success: true,
            message: 'Подключение установлено',
            siteName: 'Мой сайт на WordPress',
            wpVersion: '6.7.2',
            userId: 1,
            userDisplayName: settings.username,
        };

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
