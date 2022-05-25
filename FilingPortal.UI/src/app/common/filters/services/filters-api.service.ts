import { locationPath } from '../../../utils';
import { Injectable } from '@angular/core';

import { FilterServer, FilterSearchSettings} from '../models';

import { Observable } from 'rxjs/Observable';
import { dataToHttpParams } from '@app/functions';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable()
export class FiltersApiService {
    constructor(
        private http: HttpClient,
    ) { }

    getOptions(searchSettings: FilterSearchSettings): Observable<any> {
        return this.http
            .get(`${locationPath}/filters/getfilterdata`, { params: dataToHttpParams(searchSettings) });
    }

    getSelectFieldOptionsUrl(searchSettings) {
        const params = Object.keys(searchSettings).map(function (k) {
            return encodeURIComponent(k) + '=' + encodeURIComponent(searchSettings[k])
        }).join('&');

        return `${locationPath}/filters/getdropdowndata?${params}`;
    }

    getSelectFieldOptions(searchSettings): Observable<any> {
        return this.http
            .get(`${locationPath}/filters/getdropdowndata`, searchSettings);
    }

    getFiltersConfig(gridName: string): Observable<FilterServer[]> {
        return this.http
            .get<FilterServer[]>(`${locationPath}/settings/getFiltersConfig`, { params: new HttpParams().set('gridName', gridName) });
    }
}
