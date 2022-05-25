import { Component, OnInit } from '@angular/core';
import { VersionApiService } from "./version-api.service";
import {VersionModel} from "./VersionModel";

@Component({
  selector: 'app-version',
  templateUrl: './version.component.html'
})
export class VersionComponent implements OnInit {
  public version: VersionModel = new VersionModel();

  constructor(private versionApi: VersionApiService) { }

  ngOnInit() {
    this.getAppVersion();
  }

  getAppVersion() {
    this.versionApi.getVersion()
        .subscribe((data) => {
          if (!data) {
            return;
          }
          this.version = data as VersionModel;
        });
  }
}
