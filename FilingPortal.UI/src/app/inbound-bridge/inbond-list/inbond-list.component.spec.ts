import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InbondListComponent } from './inbond-list.component';

describe('InbondListComponent', () => {
  let component: InbondListComponent;
  let fixture: ComponentFixture<InbondListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InbondListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InbondListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
