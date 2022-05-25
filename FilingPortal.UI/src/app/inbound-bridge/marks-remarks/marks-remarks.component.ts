import { Component, OnInit, OnDestroy } from '@angular/core';
import { LookupFieldOptions } from '@common/fields/models';
import { MarksRemarksService } from '@inbound/services/marks-remarks.service';
import { MarksRemarksTemplate } from '@common/models/zones-inbond-models';
import { ListOptionModel } from '@common/fields/models/ListOptionModel';
import { Subscription } from 'rxjs';

@Component({
  selector: 'lxft-marks-remarks',
  templateUrl: './marks-remarks.component.html'
})
export class MarksRemarksComponent implements OnInit, OnDestroy {

  viewMode: boolean = false;

  templateName: string;
  private _prevTemplateName: string;

  description: string;
  marksNumbers: string;

  options: LookupFieldOptions;

  subscription: Subscription;

  constructor(private templateService: MarksRemarksService) {
  }

  ngOnInit() {
    this.subscription = this.templateService.getTemplates()
      .subscribe((x: MarksRemarksTemplate[]) => {
        const opts = new LookupFieldOptions();
        opts.options.push(...x.map(t => <ListOptionModel>{ DisplayValue: t.TemplateType, Value: t.TemplateType }));
        this.options = opts;
      });

      this.templateName = this.templateService.templateType;
      this.description = this.templateService.descriptionTemplate;
      this.marksNumbers = this.templateService.marksNumbersTemplate;
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  onSelect(): void {
    if (this.templateName !== this._prevTemplateName) {
      const template = this.templateService.getTemplate(this.templateName);
      if (template) {
        this.description = template.DescriptionTemplate;
        this.marksNumbers = template.MarksNumbersTemplate;
      }
      this.updateFields();
    }
  }

  onBlur(): void {
    this.updateFields();
  }

  private updateFields(): void {
    this.templateService.templateType = this.templateName;
    this.templateService.descriptionTemplate = this.description;
    this.templateService.marksNumbersTemplate = this.marksNumbers;
  }
}
