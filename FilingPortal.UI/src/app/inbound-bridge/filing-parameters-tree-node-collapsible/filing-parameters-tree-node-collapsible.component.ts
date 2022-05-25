import { Component, OnInit } from '@angular/core';
import { FilingParametersService } from '@inbound/services';
import { FilingParametersTreeNodeComponent } from '@inbound/filing-parameters-tree-node';

@Component({
  selector: 'lxft-filing-parameters-tree-node-collapsible',
  templateUrl: './filing-parameters-tree-node-collapsible.component.html'
})
export class FilingParametersTreeNodeCollapsibleComponent extends FilingParametersTreeNodeComponent implements OnInit {
  constructor(protected filingService: FilingParametersService) {
    super(filingService);
  }
}
