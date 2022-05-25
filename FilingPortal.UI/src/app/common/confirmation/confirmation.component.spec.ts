import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationComponent } from './confirmation.component';
import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";
import {Input} from "@angular/core";

class MockNgbActiveModal {
  @Input() text: string;
  close(){}
  dismiss(){}
}

describe('ConfirmationComponent', () => {
  let component: ConfirmationComponent;
  let fixture: ComponentFixture<ConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmationComponent ],
    });
    component = new ConfirmationComponent(new NgbActiveModal());
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('text should be equal to passed in info', () => {
    component.modalInfo = {text: 'some text'};
    component.ngOnInit();
    expect(component.modalInfo.text).toEqual(component.text);
  });
});
