import { HttpParams } from '@angular/common/http';
import * as R from 'ramda';
import { isNullOrUndefined } from 'util';

export const convertModelToFormData = (model: any, form: FormData = null, namespace = ''): FormData => {
  const formData = form || new FormData();

  for (const propertyName in model) {
    if (!model.hasOwnProperty(propertyName) || isNullOrUndefined(model[propertyName])) { continue; }
    const formKey = namespace ? `${namespace}.${propertyName}` : propertyName;
    if (model[propertyName] instanceof Date) {
      formData.append(formKey, model[propertyName].toISOString());
    } else if (model[propertyName] instanceof Array) {
      model[propertyName].forEach((element, index) => {
        const tempFormKey = `${formKey}[${index}]`;
        if (typeof element === 'object') {
          convertModelToFormData(element, formData, tempFormKey);
        } else {
          formData.append(tempFormKey, element.toString());
        }
      });
    } else
      if (model[propertyName] instanceof File) {
        const f = model[propertyName] as File;
        formData.append(formKey, f, f.name);
      } else
        if (typeof model[propertyName] === 'object' && !(model[propertyName] instanceof File)) {
          convertModelToFormData(model[propertyName], formData, formKey);
        } else {
          formData.append(formKey, model[propertyName].toString());
        }
  }
  return formData;
};

export const dataToHttpParams = (data: object): HttpParams => {
  let httpParams = new HttpParams();
  Object.keys(data).forEach(function (key) {
    httpParams = httpParams.set(key, R.isNil(data[key]) ? '' : data[key]);
  });
  return httpParams;
};
