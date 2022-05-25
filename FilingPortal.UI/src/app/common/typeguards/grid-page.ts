import { GridWithExcelTemplate } from '@common/interfaces';

export function instanceOfGridWithExcelTemplate(object: any): object is GridWithExcelTemplate {
    return typeof object.downloadTemplate === 'function';
}
