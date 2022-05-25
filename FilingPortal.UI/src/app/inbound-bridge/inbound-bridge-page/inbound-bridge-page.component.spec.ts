import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InboundBridgePageComponent } from './inbound-bridge-page.component';

describe('InboundBridgePageComponent', () => {
  let component: InboundBridgePageComponent;
  let fixture: ComponentFixture<InboundBridgePageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InboundBridgePageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InboundBridgePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
