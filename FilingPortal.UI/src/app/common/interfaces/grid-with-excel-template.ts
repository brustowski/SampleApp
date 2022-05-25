import { ElementRef } from '@angular/core';

export interface GridWithExcelTemplate {
    fileInput: ElementRef<HTMLInputElement>;
    downloadTemplate(): void;
}
