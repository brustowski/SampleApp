import { Injectable, EventEmitter } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class EventsService {
    private scrollToSubject: Subject<number> = new Subject();
    public scrollTo$: Observable<number> = this.scrollToSubject.asObservable();

    public updateGridSize$: EventEmitter<any> = new EventEmitter();

    constructor() {}

    public scrollTo(value: number): void {
        this.scrollToSubject.next(value);
    }
}
