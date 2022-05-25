import { Component, OnInit, ViewChild, forwardRef, AfterViewInit } from '@angular/core';
import * as moment from 'moment';

import { RequiredField } from '../field-required-ctrl';

import { dateFormatFull, dateFormatCompact } from '@app/utils';
import {
  INgxMyDpOptions,
  IMyDateModel,
  IMyInputFieldChanged,
  NgxMyDatePickerDirective
} from 'ngx-mydatepicker';

@Component({
  selector: 'lxft-field-date',
  templateUrl: './field-date.component.html',
  providers: [{ provide: RequiredField, useExisting: forwardRef(() => FieldDateComponent) }]
})
export class FieldDateComponent extends RequiredField implements OnInit, AfterViewInit {

  private datePatternError = `Incorrect value. Only Date (${dateFormatFull}) values are allowed`;

  public model: any;
  public settings: INgxMyDpOptions;

  @ViewChild('dp')
  public dateInput: NgxMyDatePickerDirective;

  constructor() {
    super();
  }

  ngOnInit() {
    if (this.viewMode) {
      return;
    }
    this.settings = {
      dateFormat: dateFormatFull,
      showTodayBtn: false,
      showSelectorArrow: false,
      closeSelectorOnDateSelect: true,
      closeSelectorOnDocumentClick: true,
      disableHeaderButtons: true,
      firstDayOfWeek: 'su',
      appendSelectorToBody: true
    };

    this.setStartDate();
  }

  setStartDate(): void {
    if (this.value) {
      const date = moment(this.value, [dateFormatFull.toUpperCase(), dateFormatCompact.toUpperCase()], true);
      this.model = {
        date: { year: date.year(), month: date.month() + 1, day: date.date() }
      };
    }
    this.checkRequired(this.value);
  }

  ngAfterViewInit(): void {
    if (this.dateInput) {
      this.dateInput.isDateValid();
    }
  }

  onDateCahge($event: IMyDateModel) {
    this.value = $event ? $event.formatted : $event;
    super.onChange(this.value);
    this.onBlur.emit({ event: null, oldValue: this.oldValue })
  }

  onInputChange($event: IMyInputFieldChanged) {
    const indx = this.options.errors.indexOf(this.datePatternError);
    if (indx >= 0) {
      if ($event.valid || !$event.value) {
        this.options.errors.splice(indx, 1);
      }
    } else {
      if (!$event.valid && !!$event.value) {
        this.options.errors.push(this.datePatternError);
      }
    }
  }
}
