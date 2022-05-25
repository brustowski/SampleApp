import { Component, Input, ViewChild, OnInit } from '@angular/core';
import { IMyDrpOptions, MyDateRangePicker } from 'mydaterangepicker';
import { Filter } from '../models';
import { dateFormatShort } from '@app/utils';

@Component({
  selector: 'lxft-filter-date-range',
  templateUrl: './filter-date-range.component.html'
})
export class FilterDateRangeComponent implements OnInit {

  @Input() filter: Filter;

  @ViewChild('drp')
  dateRangePicker: MyDateRangePicker;

  myDateRangePickerOptions: IMyDrpOptions = {
    dateFormat: dateFormatShort,
    width: '150px',
    height: '28px',
    showClearDateRangeBtn: false,
    firstDayOfWeek: 'su',
    selectionTxtFontSize: '12px',
  };

  constructor() { }

  private formatDate = function (val: { year: number, month: number, day: number }): string {
    const shortenYear = function (intVal: number): string {
      return '' + (intVal < 10 ? '0' + val : intVal > 99 ? (intVal % 100) : intVal);
    };

    const formatted = this.opts.dateFormat
      .replace('yyyy', val.year)
      .replace('yyy', shortenYear(val.year))
      .replace('yy', shortenYear(val.year))
      .replace('y', shortenYear(val.year))
      .replace('dd', this.preZero(val.day));
    return this.opts.dateFormat.indexOf('mmm') !== -1 ?
      formatted.replace('mmm', this.monthText(val.month)) : formatted.replace('mm', this.preZero(val.month));
  };

  ngOnInit() {
    this.dateRangePicker.formatDate = this.formatDate;
  }

}
