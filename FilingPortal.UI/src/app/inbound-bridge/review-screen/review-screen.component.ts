import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';

import {
  FilingParametersService,
  InboundRecordsValidator
} from '@inbound/services';
import { ModalService } from '@common/services';

import { InboundRecordListActions } from '@inbound/models';
import { ExpandableGridFilingComponent } from '@inbound/expandable-grid-filing';

@Component({
  selector: 'lxft-review-screen',
  templateUrl: './review-screen.component.html'
})
export class ReviewScreenComponent implements OnInit {
  public viewMode: boolean;
  public availableActions$: Observable<InboundRecordListActions>;
  public recordsCount: number;
  public showScreen: boolean = false;

  @ViewChild('grid') grid: ExpandableGridFilingComponent;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private modal: ModalService,
    private validator: InboundRecordsValidator,
    private filingService: FilingParametersService
  ) { }

  ngOnInit() {
    this.viewMode = this.route.snapshot.data['viewMode'];
    if (this.filingService.filingHeaderIds.length <= 0) {
      this.backToList();
      return;
    }
    this.showScreen = true;
    this.availableActions$ = this.filingService
      .getRecordIds()
      .do(ids => this.recordsCount = ids.length)
      .switchMap(ids => this.validator.validateSelectedRecords(ids))
      .map(result => result.Actions);
  }

  backToList() {
    this.router.navigate(['..'], { relativeTo: this.route });
  }

  save() {
    this.filingService.save().subscribe(() => {
      this.backToList();
    });
  }

  finish() {
    const message = 'Are you sure you want to create entry(s)?';
    const additionalText = 'Some of the entries has errors and will not be created. ' +
      'Press Cancel to return and fix the data or press Ok to create the valid entries';
    const allValid = this.grid.getValidationStatus().every(x => x.IsValid);

    this.modal.confirm({ text: message, additionalText: allValid ? null : additionalText }).then(confirmed => {
      if (confirmed) {
        this.filingService.startFiling().subscribe(result => {
          if (result && result.length) {
            this.filingService.filingHeaderIds = result;
            this.reload();
          } else {
            this.backToList();
          }
        });
      }
    });
  }

  public undo() {
    const message =
      'Records are prepared for mapping. Do you want to undo this process?';
    this.modal.confirm({ text: message }).then(confirmed => {
      if (confirmed) {
        this.filingService
          .cancel()
          .subscribe(() => this.backToList());
      }
    });
  }

  reload(): void {
    if (this.filingService.filingHeaderIds.length <= 0) {
      this.backToList();
      return;
    }

    this.grid.ngOnInit();
  }
}
