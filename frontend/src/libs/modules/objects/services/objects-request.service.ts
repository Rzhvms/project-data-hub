import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { IObjectPreview } from '../interfaces/object-preview.interface';

@Injectable()
export class ObjectsRequestService {
    private readonly _http: HttpClient = inject(HttpClient);

    public getObjectList(): Observable<IObjectPreview[]> {
        return this._http.get<IObjectPreview[]>('/mocks/objects-preview.mock.json');
    }
}
