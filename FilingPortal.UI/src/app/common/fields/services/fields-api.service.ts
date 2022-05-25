import { Injectable } from '@angular/core';
import { HttpService } from '../../services/http.service';
import { Observable } from 'rxjs/Observable';

import { locationPath } from '../../../utils';
import { SelectSearchSettings } from '../models/SelectSearchSettingsModel';
import { ListOptionModel } from '../models/ListOptionModel';
import { SourceType } from '../models/source-type';
import { LookupSearchSettings } from '../models/lookup-search-settings';
import { HttpClient } from '@angular/common/http';
import { dataToHttpParams } from '@app/functions';
import { LookupItemEditModel } from '@common/models';

@Injectable()
export class FieldsApiService {
  constructor(
    private http: HttpClient,
  ) { }

  getSelectFieldOptions(searchSettings: SelectSearchSettings): Observable<ListOptionModel[]> {
    return this.http
      .get<ListOptionModel[]>(`${locationPath}/filters/GetFilterData`, { params: dataToHttpParams(searchSettings) });
  }

  getLookupFieldValue(searchSettings: LookupSearchSettings): Observable<ListOptionModel[]> {
    let url: string;
    switch (searchSettings.sourceType) {
      case SourceType.Grid: url = 'grid-column-data'; break;
      case SourceType.Filter: url = 'grid-filter-data'; break;
      case SourceType.Form: url = 'data'; break;
      case SourceType.Handbook: url = 'data/handbook'; break;
    }
    const params = {
      providerName: searchSettings.sourceName,
      gridName: searchSettings.sourceName,
      fieldName: searchSettings.fieldName,
      search: searchSettings.searchText,
      limit: searchSettings.limit,
      dependOn: searchSettings.dependOn ? searchSettings.dependOn : '',
      dependValue: searchSettings.dependValue ? searchSettings.dependValue : '',
      searchByKey: searchSettings.searchByKey
    };
    return this.http
       .get<ListOptionModel[]>(`${locationPath}/lookup/${url}`, { params: dataToHttpParams(params) });
  }

  addOption(value: string, providerName: string, dependValue: any = null): Observable<ListOptionModel> {
    return this.http
      .post<ListOptionModel>(`${locationPath}/lookup/data`,
        <LookupItemEditModel>{ ProviderName: providerName, Value: value, DependValue: dependValue });
  }
}
