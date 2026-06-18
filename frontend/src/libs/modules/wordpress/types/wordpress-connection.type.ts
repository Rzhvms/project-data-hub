export type WpConnectionStatus = 'disconnected' | 'connecting' | 'connected' | 'error';

export interface IWordpressSettings {
    siteUrl: string;
    username: string;
    applicationPassword: string;
}

export interface IConnectionTestResult {
    success: boolean;
    message: string;
    siteName?: string;
    wpVersion?: string;
    userId?: number;
    userDisplayName?: string;
}

export interface ISyncStatus {
    lastSync: string | null;
    postsCount: number;
    pagesCount: number;
    mediaCount: number;
    isSyncing: boolean;
}
