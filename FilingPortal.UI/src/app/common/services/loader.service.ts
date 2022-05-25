import { Injectable, OnInit, NgZone } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class LoaderService implements OnInit {

  private operationTimeoutMs = 600;

  private loaderDisplayedSource: Subject<boolean> = new Subject();
  private loaderTimeout: any;
  private loadingReqests: number = 0;

  get loaderDisplayed$(): Observable<boolean> {
    return this.loaderDisplayedSource.asObservable();
  }

  constructor(private zone: NgZone) { }

  ngOnInit() {
    this.loaderDisplayedSource.next(false);
  }

  showLoader() {
    this.loadingReqests++;
    if (this.loadingReqests === 1) {
      this.loaderTimeout = setTimeout(() => {
        this.zone.run(() => {
          this.loaderDisplayedSource.next(true);
        });
      }, this.operationTimeoutMs);
    }
  }

  hideLoader() {
    if (this.loadingReqests > 0) {
      this.loadingReqests--;
      if (this.loadingReqests === 0) {
        clearTimeout(this.loaderTimeout);
        this.loaderDisplayedSource.next(false);
      }
    }
  }
}
