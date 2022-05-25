import { Component, OnInit, Input, AfterViewChecked, ChangeDetectorRef, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable, Subscription } from 'rxjs';

import { FilingParametersService } from '@inbound/services';
import { FilingConfigurationTree, InboundType, FieldsFilterSettings } from '@inbound/models';
import { ExpandableGridFilingComponent } from '@inbound/expandable-grid-filing/expandable-grid-filing.component';
import { Filter } from '@common/filters/models';
import { EventsService } from '@common/services';
import { isUndefined, isNullOrUndefined } from 'util';

@Component({
  selector: 'lxft-expandable-grid-detailed-row',
  templateUrl: './expandable-grid-detailed-row.component.html'
})
export class ExpandableGridDetailedRowComponent implements OnInit, AfterViewChecked, OnDestroy {
  @Input() filingHeaderId: number;
  @Input() isConsolidated: boolean;

  @ViewChild('dataPanel') dataPanelView: ElementRef;

  public filterSettings: FieldsFilterSettings;
  public configuration$: Observable<FilingConfigurationTree>;
  public viewMode: boolean = false;
  public inboundType: InboundType;

  public InboundType = InboundType;
  hasMarkedForReviewFields: boolean = false;
  subscription: Subscription;

  get isRailManifestVisible(): boolean {
    return this.inboundType === InboundType.Rail;
  }

  get isMarksRemarksVisible(): boolean {
    return this.inboundType === InboundType.Inbond;
  }

  constructor(
    private route: ActivatedRoute,
    private paramsService: FilingParametersService,
    private grid: ExpandableGridFilingComponent,
    private changeDetectorRef: ChangeDetectorRef,
    private events: EventsService) { }

  ngOnInit() {
    this.filterSettings = new FieldsFilterSettings();
    this.viewMode = this.route.snapshot.data['viewMode'];
    this.inboundType = this.route.snapshot.data['type'];
    this.configuration$ = this.paramsService.filingConfiguration$;
    this.paramsService.setFilingHeaderId(this.filingHeaderId);

    this.subscription = this.paramsService.filingConfiguration$
      .filter(x => !isNullOrUndefined(x))
      .subscribe(x => {
        this.hasMarkedForReviewFields = x.fields.find(f => f.markedForFiltering) !== undefined;
      });

    this.onSizeChange(100);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  updateGrid(): void {
    const errors = this.paramsService.validate();
    this.grid.updateRowStatus(errors);
  }

  updateDetailRowHeight() {
    this.changeDetectorRef.markForCheck();
  }

  onSizeChange(timeoutValue: number = 0) {
    setTimeout(() => {
      this.updateDetailRowHeight();
    }, timeoutValue);
  }

  ngAfterViewChecked() {
    // Check if the table size has changed,
    const table = this.grid.gridTable;
    if (table && table.recalculate && (this.dataPanelView.nativeElement.offsetHeight !== table.rowDetail.rowHeight)) {
      table.rowDetail.rowHeight = this.dataPanelView.nativeElement.offsetHeight;
      this.events.updateGridSize$.emit();
    }
  }

  getRailExportContainersFilter(): Filter[] {
    const filter = new Filter();
    filter.fieldName = 'FilingHeaderId';
    filter.value = [this.filingHeaderId];
    filter.operand = 'equals';

    return [filter];
  }
}
