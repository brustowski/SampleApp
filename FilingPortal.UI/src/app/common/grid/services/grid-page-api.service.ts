import { Injectable } from '@angular/core';
import { locationPath } from '../../../utils';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class GridPageApiService {
  constructor(
    private http: HttpClient
  ) { }

  listSearch(path, params): Promise<any> {
    return this.http
      .post(`${locationPath}/${path}/search`, params)
      .toPromise();
  }

  getTotalRows(path): Observable<any> {
    return this.http
      .get(`${locationPath}/${path}/getTotalRows`);
  }

  getTotalMatches(path, params): Promise<any> {
    return this.http
      .post(`${locationPath}/${path}/getTotalMatches`, params)
      .toPromise();
  }

  getGridColumnsConfig(gridName): Observable<any> {
    return this.http
      .get(`${locationPath}/settings/getgridconfig?gridName=${gridName}`);
  }

  exportToExcel(gridName, data): void {
    const str = `${locationPath}/reports/DownloadExportToExcel?gridName=${gridName}&data=` + btoa(JSON.stringify(data));
    window.open(str, '_blank');
  }

  downloadTemplate(gridName: string): void {
    const str = `${locationPath}/file-template/by-grid-name/${gridName}`;
    window.open(str, '_blank');
  }

  // todo: replace all download template methods with generic one
  downloadRuleTemplate(gridName: string): void {
    const str = `${locationPath}/imports/templates/${gridName}`;
    window.open(str, '_blank');
  }
}
