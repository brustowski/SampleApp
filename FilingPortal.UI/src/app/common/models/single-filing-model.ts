import { InboundRecordFileModel, InboundRecordConfigurationServer } from '@inbound/models';
import { isSimpleField } from '@common/typeguards/inbound-record-field';
import { InboundRecordField } from '.';

export class SingleFilingModel {
    model: InboundRecordFileModel;
    config: InboundRecordConfigurationServer;

    public constructor(model: InboundRecordFileModel, config: InboundRecordConfigurationServer) {
        this.model = model;
        this.config = config;
    }

    public getCurrentConfig(): InboundRecordConfigurationServer {
        const newConfig = { ...this.config };

        newConfig.AdditionalParameters.forEach(x => {
            if (isSimpleField(x)) {
                this.UpdateField(x);
            } else {
                x.Fields.forEach(field => {
                    this.UpdateField(field);
                });
            }
        });

        newConfig.CommonData.forEach(section => {
            section.Fields.forEach(x => {
                if (isSimpleField(x)) {
                    this.UpdateField(x);
                } else {
                    x.Fields.forEach(field => {
                        this.UpdateField(field);
                    });
                }
            });
        });

        newConfig.Documents = [];
        this.model.Documents.forEach(x => {
            newConfig.Documents.push(
                {
                    Description: x.Description,
                    FilingHeaderId: this.model.FilingHeaderId,
                    Id: x.Id,
                    IsManifest: x.IsManifest,
                    Name: x.Name,
                    Status: x.Status,
                    Type: x.Type
                }
            );
        });

        return newConfig;
    }

    private UpdateField(field: InboundRecordField) {
        const currentValue = this.model.Parameters.find(y => y.Id === field.Id);
        if (currentValue) {
            field.DefaultValue = currentValue.Value;
        } else {
            console.log(`Parameter with Id=${field.Id} and title=${field.Title} not found`);
        }
    }
}
