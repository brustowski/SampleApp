import * as R from 'ramda';
import { paginationInfo } from '../../../utils';
import { Page, Sort } from './page';
import { Filter, FilterParsed, FilterParsedConfig } from '@common/filters/models';
import { IMyDateRangeModel } from 'mydaterangepicker';

export class PageParams {
  PagingSettings: any = {
    PageNumber: 1,
    PageSize: paginationInfo.pageSize
  };

  SortingSettings: { Field: string, SortOrder: string | number } = {
    Field: null,
    SortOrder: 0
  };

  FilterSettings: FilterParsedConfig;

  constructor(page: Page, sort: Sort, filters: Filter[]) {
    if (page) {
      this.PagingSettings.PageNumber = page.pageNumber;
      this.PagingSettings.PageSize = page.size;
    }
    if (sort) {
      this.SortingSettings.SortOrder = sort.order;
      this.SortingSettings.Field = sort.column;
    }
    this.FilterSettings = PageParams.getParsedFilters(filters);
  }

  public static getParsedFilters(filters: Filter[]): FilterParsedConfig {
    const hasValue = R.has('Value');
    const hasDisplayValue = R.has('DisplayValue');
    const fillFilterWithArrayOfValues = (item: Filter): any[] => {
      return R.map(v => {
        return {
          Value: hasValue(v) ? v.Value : v,
          DisplayValue: hasDisplayValue(v) ? v.DisplayValue : v
        };
      }, item.value);
    };

    const getFilterFormatDateRange = (value: IMyDateRangeModel): string[] => {
      return [
        `${value.beginDate.year}-${value.beginDate.month}-${value.beginDate.day}`,
        `${value.endDate.year}-${value.endDate.month}-${value.endDate.day} 23:59`
      ];
    };

    const getFilterFormatDate = value => {
      const date = new Date(value);
      let newDate = new Date().setUTCHours(0);
      newDate = new Date(newDate).setUTCMinutes(0);
      newDate = new Date(newDate).setUTCSeconds(0);
      newDate = new Date(newDate).setUTCMilliseconds(0);
      newDate = new Date(newDate).setFullYear(date.getFullYear());
      newDate = new Date(newDate).setMonth(date.getMonth());
      newDate = new Date(newDate).setDate(date.getDate());

      return new Date(newDate);
    };

    if (!filters) {
      return { Filters: [] };
    }

    const parsedFilters = filters.map(item => {
      let values: any[] = [];
      if (R.is(Array, item.value)) {
        values = fillFilterWithArrayOfValues(item);
      } else {
        switch (item.type) {
          case 'date':
            values = [
              {
                Value: getFilterFormatDate(item.value),
                DisplayValue: item.value.DisplayValue
              }
            ];
            break;
          case 'date-range':
            const v = <IMyDateRangeModel>item.value;
            values = [
              {
                Value: getFilterFormatDateRange(v),
                DisplayValue: v.formatted
              }
            ];
            break;
          case 'select':
            values = [item.value];
            break;
          default:
            values = [
              {
                Value: item.value,
                DisplayValue: item.value
              }
            ];
            break;
        }
      }
      return <FilterParsed>{
        FieldName: item.fieldName,
        Operand: item.operand,
        Values: values
      };
    });

    return {
      Filters: parsedFilters
    };
  }
}
