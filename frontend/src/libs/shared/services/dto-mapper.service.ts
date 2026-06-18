import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

const LS_PREFIX = 'data-hub-mock:';

export interface DbCollectionConfig {
    name: string;
    seedUrl: string;
}

@Injectable({ providedIn: 'root' })
export class DtoMapperService {
    private readonly _http: HttpClient = inject(HttpClient);
    private readonly _cache: Map<string, any[]> = new Map();
    private readonly _seeded: Set<string> = new Set();

    public getCollection<T>(config: DbCollectionConfig): Observable<T[]> {
        const cached: T[] | undefined = this._cache.get(config.name) as T[] | undefined;

        if (cached) {
            return of(cached);
        }

        return this._seedFromFile<T>(config).pipe(
            tap((data: T[]) => {
                this._cache.set(config.name, data);
                this._seeded.add(config.name);
            }),
        );
    }

    public getById<T extends { id: string }>(config: DbCollectionConfig, id: string): Observable<T | null> {
        return this.getCollection<T>(config).pipe(
            map((items: T[]) => items.find((item: T) => item.id === id) ?? null),
        );
    }

    public create<T extends { id: string }>(config: DbCollectionConfig, item: T): Observable<T> {
        const collection: T[] = (this._cache.get(config.name) as T[]) ?? [];

        collection.push(item);
        this._cache.set(config.name, collection);
        this._persist(config.name, collection);

        return of(item);
    }

    public update<T extends { id: string }>(config: DbCollectionConfig, id: string, changes: Partial<T>): Observable<T | null> {
        const collection: T[] = (this._cache.get(config.name) as T[]) ?? [];
        const index: number = collection.findIndex((item: T) => item.id === id);

        if (index === -1) {
            return of(null);
        }

        collection[index] = { ...collection[index], ...changes };
        this._cache.set(config.name, collection);
        this._persist(config.name, collection);

        return of(collection[index]);
    }

    public delete(config: DbCollectionConfig, id: string): Observable<boolean> {
        const collection: any[] = this._cache.get(config.name) ?? [];
        const index: number = collection.findIndex((item: any) => item.id === id);

        if (index === -1) {
            return of(false);
        }

        collection.splice(index, 1);
        this._cache.set(config.name, collection);
        this._persist(config.name, collection);

        return of(true);
    }

    public seedCollection<T extends { id: string }>(name: string, data: T[]): void {
        this._cache.set(name, data);
        this._seeded.add(name);
        this._persist(name, data);
    }

    private _seedFromFile<T>(config: DbCollectionConfig): Observable<T[]> {
        const persisted: string | null = localStorage.getItem(LS_PREFIX + config.name);

        if (persisted) {
            try {
                return of(JSON.parse(persisted) as T[]);
            } catch {
                /* fall through to seed from file */
            }
        }

        return this._http.get<T[]>(config.seedUrl).pipe(
            catchError(() => of([] as T[])),
        );
    }

    private _persist(name: string, data: any[]): void {
        try {
            localStorage.setItem(LS_PREFIX + name, JSON.stringify(data));
        } catch {
            /* localStorage full or unavailable */
        }
    }
}
