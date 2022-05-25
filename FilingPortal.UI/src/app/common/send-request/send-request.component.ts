import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../services/authenticationService';
import { SendRequestModel, PageMode } from '../models/send-request-model';
import { AppUserViewModel } from '@common/models';

@Component({
  selector: 'lxft-send-request',
  templateUrl: 'send-request.component.html'
})
export class SendRequestComponent implements OnInit {
  requestModel: SendRequestModel;
  messageBank = {};

  ngOnInit(): void {
    this.messageBank[PageMode.new] = 'You don\'t have access to this site. You can request access using the form below.';
    this.messageBank[PageMode.waiting] = 'Your request is being processed. Please try again later';
    this.messageBank[PageMode.inactive] = 'Your account is disabled, please contact Administrator';
    this.messageBank[PageMode.requestSent] = 'Your request was sent to Administrator. Please try to connect later';

    this.authenticationService.getUser().subscribe(x => {
      this.requestModel = this.LoadUserModel(x);
    });
  }

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) { }

  private LoadUserModel(user: AppUserViewModel): SendRequestModel {
    if (!user) {
      return { mode: PageMode.new, requestInfo: '' };
    }
    const pageMode = this.GetPageMode(user.Status);
    if (!pageMode) {
      this.router.navigate(['/']);
    }
    return { mode: pageMode };
  }

  private GetPageMode(status: string): PageMode {
    switch (status) {
      case 'Inactive': return PageMode.inactive;
      case 'Waiting': return PageMode.waiting;
      default: return null;
    }
  }

  SaveUserModel() {
    this.authenticationService.sendRequest(this.requestModel.requestInfo || '').subscribe(
      x => {
        this.requestModel.mode = PageMode.requestSent;
      });
  }
}
