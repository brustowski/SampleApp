import { FieldOptions } from '@common/fields/models/FieldOptions';
import { InboundRecordDocumentStatus } from './inbound-record-document-status';
import { InboundRecordDocument } from './inbound-record-document';
import { FileItem } from 'ng2-file-upload';
import { InboundRecordField, ComplexInboundRecordField } from '@common/models';
import { LookupFieldOptions } from '@common/fields/models';

export class InboundRecordParameter {
  id: any = null;
  recordId: number;
  parentRecordId: number;
  name: string;
  title: string;
  value: any;
  section: string;
  type: string;
  isDisabled: boolean = false;
  options: FieldOptions | LookupFieldOptions = new FieldOptions();
  additionalFields: InboundRecordParameter[] = [];
  confirmationNeeded: boolean = false;
  markedForFiltering: boolean = false;
}

export class InboundRecordConfigurationServer {
  AdditionalParameters: (InboundRecordField | ComplexInboundRecordField)[];
  CommonData: InboundRecordCommonDataServer[];
  Documents: InboundRecordDocumentServer[];
}

export class InboundRecordParameterBuilder {
  model: InboundRecordParameter;
  constructor() {}

  create() {
    this.model = new InboundRecordParameter();
    return this;
  }

  id(id = null) {
    this.model.id = id;
    return this;
  }
  recordId(id: number) {
    this.model.recordId = id;
    return this;
  }
  parentRecordId(id: number) {
    this.model.parentRecordId = id;
    return this;
  }

  title(title = '') {
    this.model.title = title;
    return this;
  }

  name(value: string = '') {
    this.model.name = value;
    return this;
  }

  section(value: string) {
    this.model.section = value;
    return this;
  }

  defValue(value = '') {
    this.model.value = value;
    return this;
  }

  type(type = '') {
    this.model.type = type;
    return this;
  }

  disable(value = false) {
    this.model.isDisabled = value;
    return this;
  }

  options(options = new FieldOptions()) {
    this.model.options = options;
    return this;
  }

  additionalFields(parameters: InboundRecordParameter[]) {
    this.model.additionalFields = parameters;
    return this;
  }

  confirmationNeeded(value: boolean): InboundRecordParameterBuilder {
    this.model.confirmationNeeded = value;
    return this;
  }

  markedForFiltering(value: boolean): InboundRecordParameterBuilder {
    this.model.markedForFiltering = value;
    return this;
  }

  build() {
    return this.model;
  }
}

export class InboundRecordDocumentBuilder {
  model: InboundRecordDocument;
  constructor() {}

  create() {
    this.model = new InboundRecordDocument();
    return this;
  }

  id(id: number) {
    this.model.id = id;
    return this;
  }

  filingHeaderId(id: number) {
    this.model.filingHeaderId = id;
    return this;
  }

  name(name: string) {
    this.model.name = name;
    return this;
  }

  type(type: string) {
    this.model.type = type;
    return this;
  }

  description(description = '') {
    this.model.description = description;
    return this;
  }

  status(status: InboundRecordDocumentStatus) {
    this.model.status = status;
    return this;
  }

  isManifest(isManifest: boolean = false) {
    this.model.isManifest = isManifest;
    return this;
  }

  file(value: FileItem = null) {
    this.model.fileObj = value;
    return this;
  }

  build() {
    return this.model;
  }
}

export class InboundRecordDocumentServer {
  public Id: number;
  public FilingHeaderId: number;
  public Name: string;
  public Type: string;
  public Description: string;
  public Status: InboundRecordDocumentStatus;
  public IsManifest: boolean;
}

export class InboundRecordDocumentModel {
  public Id: number;
  public Name: string;
  public Type: string;
  public Description: string;
  public Data: any;
  public Status: InboundRecordDocumentStatus;
  public IsManifest: boolean;
}

export class InboundRecordFileModel {
  FilingHeaderId: number;
  Parameters: InboundRecordParameterModel[];
  Documents: InboundRecordDocumentModel[];
}

export class InboundRecordParameterModel {
  Id: number;
  RecordId: number;
  ParentRecordId: number;
  Value: string;
}

export class InboundRecordCommonDataServer {
  SectionName: string;
  Fields: (InboundRecordField | ComplexInboundRecordField)[];
}

export class InboundRecordCommonData {
  sectionName: string;
  fields: InboundRecordParameter[] = [];
  get hasErrors(): boolean {
    return this.fields && this.fields.some(f => f.options.hasErrors());
  }
}

export class InboundRecordCommonDataBuilder {
  private model: InboundRecordCommonData;
  constructor() {}

  create() {
    this.model = new InboundRecordCommonData();
    return this;
  }

  sectionName(name = '') {
    this.model.sectionName = name;
    return this;
  }

  fields(fields: InboundRecordParameter[] = []) {
    this.model.fields = fields;
    return this;
  }

  build() {
    return this.model;
  }
}


