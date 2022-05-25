import { InboundRecordDocument } from './inbound-record-document';
import { InboundRecordParameter } from './inbound-record-parameter';
import { TreeNode } from './tree-node';

export class FilingConfigurationTree {
  filingHeaderId: number;
  manualTabTitle: string;
  form7501: TreeNode<InboundRecordParameter>;
  formRuleDrivenData: TreeNode<InboundRecordParameter>;
  documents: TreeNode<InboundRecordDocument>;
  unallocatedFields: InboundRecordParameter[];

  get fields(): InboundRecordParameter[] {
    return [... this.form7501.getData(true), ... this.formRuleDrivenData.getData(true), ... this.unallocatedFields];
  }
}
