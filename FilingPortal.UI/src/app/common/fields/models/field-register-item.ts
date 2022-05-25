export class FieldRegisterItem {
    constructor(public fieldName: string, public valueAccessor: () => any, public setValueAccessor: (value: any) => void) {}
}
