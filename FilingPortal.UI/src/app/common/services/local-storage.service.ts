import { Injectable } from '@angular/core';
import { LocalStorageService as LocalStorage } from 'angular-2-local-storage';

@Injectable()
export class LocalStorageService {
    constructor(
        private localStorage: LocalStorage
    ) {
    }

    set(key, val) {
        this.localStorage.set(key, val);
    }

    get(key): any {
        return this.localStorage.get(key);
    }

    clearAll() {
        this.localStorage.clearAll();
    }
}
