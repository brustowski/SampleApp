import { Injectable } from '@angular/core';

import { Observable, of, BehaviorSubject } from 'rxjs';
import * as R from 'ramda';

import { ToastrService } from 'ngx-toastr';

import { InboundRecordsApiService } from './inbound-records-api.service';

import {
  TreeNode,
  InboundRecordFileModel,
  InboundRecordDocumentModel,
  InboundRecordParameterModel,
  InboundRecordDocument,
  FilingConfiguration,
  FilingConfigurationField,
  FilingConfigurationSection,
  FilingConfigurationTree,
  InboundRecordParameter,
  FieldErrors
} from '@inbound/models';

import { InboundRecordsValidator } from './inbound-records.validator';
import { convertParameter } from '@inbound/mappings';
import { InboundConfigurationService } from './inbound-configuration.service';
import { isObject } from 'util';
import { FilingHeaderConfirmation } from '@common/models';

@Injectable({
  providedIn: 'root'
})
export class FilingParametersService {
  private data: Map<number, FilingConfigurationTree> = new Map();
  public filingHeaderIds: number[] = [];

  private filingConfiguration = new BehaviorSubject<FilingConfigurationTree>(null);
  public filingConfiguration$ = this.filingConfiguration.asObservable();

  constructor(private apiService: InboundRecordsApiService
    , private validator: InboundRecordsValidator
    , private toastr: ToastrService
    , private mappingsService: InboundConfigurationService) { }

  public clear() {
    this.filingHeaderIds = [];
    this.data.clear();
    this.filingConfiguration.next(null);
  }

  public getRecordIds(): Observable<number[]> {
    return this.apiService.getRecordIds(this.filingHeaderIds);
  }

  public setFilingHeaderId(filingHeaderId: number): void {
    this.getConfiguration(filingHeaderId).subscribe(config => {
      this.filingConfiguration.next(config);
    });
  }

  private getConfiguration(filingHeaderId: number): Observable<FilingConfigurationTree> {
    if (filingHeaderId) {
      if (this.data.has(filingHeaderId)) {
        return of(this.data.get(filingHeaderId));
      }
      return this.apiService
        .getFilingConfiguration(filingHeaderId)
        .map(config => this.convert(config))
        .do(x => this.data.set(filingHeaderId, x))
        .map(config => this.convertToFileModel(config))
        .switchMap(model => this.apiService.recalculateFieldValues(model))
        .do(result => {
          if (result) {
            const data = this.data.get(result.FilingHeaderId).fields;
            data.forEach(x => {
              const foundData = result.Parameters.find(y => y.Id === x.id && y.RecordId === x.recordId);
              if (foundData) {
                x.value = foundData.Value;
              }
            });
          }
        }).switchMap(() => of(this.data.get(filingHeaderId)));
    }
  }

  private convert(config: FilingConfiguration): FilingConfigurationTree {
    const result = new FilingConfigurationTree();
    result.manualTabTitle = this.mappingsService.getManualTabTitle();
    result.filingHeaderId = config.filingHeaderId;
    result.form7501 = this.get7501Form(config);
    result.formRuleDrivenData = this.getRuleDrivenDataForm(config);
    result.documents = this.getDocuments(config);
    result.unallocatedFields = this.getUnallocated(config);
    return result;
  }

  private get7501Form(config: FilingConfiguration): TreeNode<InboundRecordParameter> {
    const root = this.buildTree<InboundRecordParameter>(config);
    const stack: TreeNode<InboundRecordParameter>[] = [root];
    const fields = config.fields.filter(f => f.isVisibleOn7501);
    while (stack.length) {
      const node = stack.pop();
      node.children.forEach(child => stack.push(child));
      node.data = this.getNodeFields(node, fields);
    }
    return root;
  }
  private getNodeFields(node: TreeNode<InboundRecordParameter>, fields: FilingConfigurationField[]): InboundRecordParameter[] {
    return fields
    .filter(f => f.sectionName === node.name && f.recordId === node.id)
    .sort((l, r) => l.order - r.order)
    .map(f => this.convertField(f));
  }

  private buildTree<T>(filingConfiguration: FilingConfiguration): TreeNode<T> {
    if (!filingConfiguration) {
      return new TreeNode<T>();
    }
    const nodes: TreeNode<T>[] = this.getNodes<T>(filingConfiguration);
    const rootSection = filingConfiguration.sections.find(s => !s.parentId);
    const root = new TreeNode<T>();
    root.id = filingConfiguration.filingHeaderId;
    root.name = rootSection.name;
    root.parentId = 0;
    root.title = rootSection.title;
    const stack: { node: TreeNode<T>; section: FilingConfigurationSection }[] = [{ node: root, section: rootSection }];
    while (stack.length) {
      const entry = stack.pop();
      const sections = filingConfiguration.sections.filter(s => s.parentId === entry.section.id);
      sections.forEach(s => {
        const availableNodes = nodes.filter(n => n.name === s.name && n.parentId === entry.node.id);
        if (!availableNodes.length) {
          const node = new TreeNode<T>();
          node.id = entry.node.id;
          node.parentId = entry.node.id;
          node.name = s.name;
          node.title = s.title;
          availableNodes.push(node);
        }
        availableNodes.forEach(n => {
          n.actions = { Delete: !s.isSingleSection, Add: !s.isSingleSection };
          n.displayAsGrid = s.displayAsGrid;
          entry.node.children.push(n);
          stack.push({ node: n, section: s });
        });
      });
    }
    return root;
  }

  private getNodes<T>(filingConfiguration: FilingConfiguration) {
    const nodes: TreeNode<T>[] = [];
    filingConfiguration.fields.forEach(f => {
      const idx = nodes.findIndex(s => s.id === f.recordId && s.name === f.sectionName);
      if (idx === -1) {
        const node = new TreeNode<T>();
        node.id = f.recordId;
        node.parentId = f.parentRecordId;
        node.name = f.sectionName;
        node.title = f.sectionTitle;
        nodes.push(node);
      }
    });
    return nodes;
  }

  private convertField(field: FilingConfigurationField): InboundRecordParameter {
    const f = convertParameter(field.Field);
    f.name = field.name;
    if (f.additionalFields.length) {
      f.additionalFields.forEach(x => this.ensureDependency(x));
    } else {
      this.ensureDependency(f);
    }

    f.section = field.sectionName;
    return f;
  }

  private ensureDependency(f: InboundRecordParameter) {
    f.options.name = `${f.recordId}_${f.parentRecordId}_${f.name}`;
    if (f.options.dependsOn) {
      f.options.dependsOn = `${f.recordId}_${f.parentRecordId}_${f.options.dependsOn}`;
    }
  }

  private getRuleDrivenDataForm(config: FilingConfiguration): TreeNode<InboundRecordParameter> {
    const root = this.buildTree<InboundRecordParameter>(config);
    const stack: TreeNode<InboundRecordParameter>[] = [root];
    const fields = config.fields.filter(f => f.isVisibleOnRuleDrivenData);
    while (stack.length) {
      const node = stack.pop();
      node.children.forEach(child => stack.push(child));
      node.data = this.getNodeFields(node, fields);
    }
    return root;
  }

  private getDocuments(config: FilingConfiguration): TreeNode<InboundRecordDocument> {
    const root = new TreeNode<InboundRecordDocument>();
    const rootSection = config.sections.find(s => !s.parentId);
    root.id = config.filingHeaderId;
    root.name = rootSection.name;
    root.parentId = 0;
    root.title = rootSection.title;
    const documentNode = new TreeNode<InboundRecordDocument>();
    documentNode.id = root.id;
    documentNode.parentId = root.id;
    documentNode.name = 'documents';
    documentNode.title = 'Documents';
    documentNode.data = config.documents;
    root.children.push(documentNode);
    return root;
  }

  private getUnallocated(config: FilingConfiguration): InboundRecordParameter[] {
    const fields = config.fields.filter(f => !f.isVisibleOn7501 && !f.isVisibleOnRuleDrivenData);
    return fields.map(f => this.convertField(f));
  }

  public save(): Observable<any> {
    const models = this.convertAllToFileModel();

    return this.apiService.save({ Models: models }).map(result => {
      result.Messages.forEach(m => this.toastr.error(m));
      return result.Results.filter(r => !r.IsValid).map(r => r.FilingHeaderId);
    });
  }

  private convertAllToFileModel(): InboundRecordFileModel[] {
    const results: InboundRecordFileModel[] = [];
    this.filingHeaderIds.forEach(id => {
      let model: InboundRecordFileModel;
      if (this.data.has(id)) {
        const clientModel = this.data.get(id);
        model = this.convertToFileModel(clientModel);
      } else {
        model = new InboundRecordFileModel();
        model.FilingHeaderId = id;
      }
      results.push(model);
    });
    return results;
  }

  private convertToFileModel(clientModel: FilingConfigurationTree): InboundRecordFileModel {
    const model = new InboundRecordFileModel();
    model.FilingHeaderId = clientModel.filingHeaderId;
    model.Parameters = [];
    this.data.get(model.FilingHeaderId).fields.forEach(field => {
      model.Parameters.push(...this.fieldToServerModel(field));
    });
    model.Documents = this.data.get(model.FilingHeaderId).documents.getData(true).map(x => this.documentToServerModel(x));
    return model;
  }

  private fieldToServerModel(param: InboundRecordParameter): InboundRecordParameterModel[] {
    const result = [];
    if (param.additionalFields.length) {
      // We have complex field here
      param.additionalFields.forEach(field => {
        result.push(...this.fieldToServerModel(field));
      });
    } else {
      const model = new InboundRecordParameterModel();
      model.Id = param.id;
      model.RecordId = param.recordId;
      model.Value = isObject(param.value) ? JSON.stringify(param.value) : param.value;
      model.ParentRecordId = param.parentRecordId;
      result.push(model);
    } return result;
  }

  private documentToServerModel(document: InboundRecordDocument): InboundRecordDocumentModel {
    const model = new InboundRecordDocumentModel();
    model.Id = document.id;
    model.Name = document.name;
    model.Type = document.type;
    model.Description = document.description;
    model.Data = document.fileObj ? document.fileObj._file : null;
    model.Status = document.status;
    model.IsManifest = false;
    return model;
  }

  public cancel(): Observable<any> {
    return this.apiService.undo(this.filingHeaderIds);
  }

  public startFiling(): Observable<number[]> {
    const models = this.convertAllToFileModel();
    return this.apiService.file({ Models: models }).map(result => {
      result.Messages.forEach(m => this.toastr.error(m));
      return result.Results.filter(r => !r.IsValid).map(r => r.FilingHeaderId);
    });
  }

  public add(nodeName: string, parentId: number): void {
    const config = this.filingConfiguration.getValue();
    this.apiService.addConfiguration(config.filingHeaderId, nodeName, parentId).subscribe(filingConfiguration => {
      this.updateFilingConfiguration(config, filingConfiguration);
      this.recalculateFields();
      this.data.set(config.filingHeaderId, config);
      this.filingConfiguration.next(config);
    });
  }

  private updateFilingConfiguration(config: FilingConfigurationTree, filingConfiguration: FilingConfiguration): void {
    const nodes: TreeNode<InboundRecordParameter>[] = this.getNodes<InboundRecordParameter>(filingConfiguration);
    nodes.forEach(node => {
      const node7501 = this.addNode(config.form7501, node, filingConfiguration.sections);
      node7501.data = this.getNodeFields(node, filingConfiguration.fields.filter(f => f.isVisibleOn7501));
      // filingConfiguration.fields
      //   .filter(f => f.isVisibleOn7501)
      //   .filter(f => f.sectionName === node.name && f.recordId === node.id)
      //   .map(f => this.convertField(f));
      const nodeRDD = this.addNode(config.formRuleDrivenData, node, filingConfiguration.sections);
      nodeRDD.data = this.getNodeFields(node, filingConfiguration.fields.filter(f => f.isVisibleOnRuleDrivenData));
      // filingConfiguration.fields
      //   .filter(f => f.isVisibleOnRuleDrivenData)
      //   .filter(f => f.sectionName === node.name && f.recordId === node.id)
      //   .map(f => this.convertField(f));
    });
  }

  private addNode(
    rootNode: TreeNode<InboundRecordParameter>,
    node: TreeNode<InboundRecordParameter>,
    sections: FilingConfigurationSection[]
  ): TreeNode<InboundRecordParameter> {
    const section = sections.find(s => s.name === node.name);
    try {
      const parentSection = sections.find(s => s.id === section.parentId);
      const parentNode = rootNode.getNode(parentSection.name, node.parentId);
      const childNode = R.clone(node);
      childNode.actions = { Delete: !section.isSingleSection, Add: !section.isSingleSection };
      childNode.displayAsGrid = section.displayAsGrid;
      parentNode.children.push(childNode);
      return childNode;
    } catch {
      return null;
    }
  }

  public refresh(nodeName: string, recordId: number): void {
    const config = this.filingConfiguration.getValue();
    this.apiService.getConfiguration(config.filingHeaderId, nodeName, recordId).subscribe(filingConfiguration => {
      this.deleteNode(config.form7501, nodeName, recordId);
      this.deleteNode(config.formRuleDrivenData, nodeName, recordId);
      this.updateFilingConfiguration(config, filingConfiguration);
      this.recalculateFields();
      this.data.set(config.filingHeaderId, config);
      this.filingConfiguration.next(config);
    });
  }

  public delete(nodeName: string, recordId: number): void {
    const config = this.filingConfiguration.getValue();
    this.apiService.deleteConfiguration(config.filingHeaderId, nodeName, recordId).subscribe(() => {
      this.deleteNode(config.form7501, nodeName, recordId);
      this.deleteNode(config.formRuleDrivenData, nodeName, recordId);
      this.data.set(config.filingHeaderId, config);
      this.filingConfiguration.next(config);
      this.recalculateFields();
    });
  }

  private deleteNode(root: TreeNode<InboundRecordParameter>, nodeName: string, nodeId: number): void {
    const stack: TreeNode<InboundRecordParameter>[] = [root];
    while (stack) {
      const node = stack.pop();
      const idx = node.children.findIndex(n => n.name === nodeName && n.id === nodeId);
      if (idx !== -1) {
        node.children.splice(idx, 1);
        return;
      }
      stack.push(...node.children);
    }
  }

  public validate(): FieldErrors[] {
    const config = this.filingConfiguration.getValue();
    const fieldErrors = this.validator.validateFields(config.fields);
    const documentErrors = this.validator.validateDocuments(config.documents.getData(true));
    return [...fieldErrors, ...documentErrors];
  }

  getCurrentModel(): InboundRecordFileModel {
    const config = this.filingConfiguration.getValue();
    return this.convertToFileModel(config);
  }

  getCurrentFilingHeaderId(): number {
    const config = this.filingConfiguration.getValue();
    return config.filingHeaderId;
  }

  recalculateFields() {
    const config = this.filingConfiguration.getValue();
    const model = this.convertToFileModel(config);
    this.apiService.recalculateFieldValues(model).subscribe(result => {
      if (result) {
        const data = this.data.get(result.FilingHeaderId).fields;
        data.forEach(x => this.setRecalculatedValue(x, result.Parameters));
      }
    });
  }

  private setRecalculatedValue(field: InboundRecordParameter, updatedParams: InboundRecordParameterModel[]): void {
    const foundData = updatedParams.find(y => y.Id === field.id && y.RecordId === field.recordId);
    if (foundData) {
      field.value = foundData.Value;
    }
    if (field.additionalFields) {
      field.additionalFields.forEach(additionalField => this.setRecalculatedValue(additionalField, updatedParams));
    }
  }

  getFieldByName(fieldName: string, sectionnName: string): InboundRecordParameter {
    const config = this.filingConfiguration.getValue();
    return config ? config.fields.find(f => f.name === fieldName && f.section === sectionnName) : null;
  }

  updateConfirmationStatus(confirmations: FilingHeaderConfirmation[]): Observable<FilingHeaderConfirmation[]> {
    return this.apiService.updateConfirmationStatus(confirmations);
  }

  public get ConfirmationAvailable(): boolean {
    return !!this.mappingsService.getFilingHeaderConfirmationPath();
  }

  public get CanExclude(): boolean {
    return !!this.mappingsService.getExcludeAvailable();
  }
}

