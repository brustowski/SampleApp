import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ZonesEntryListComponent } from './zones-entry-list.component';

describe('InbondListComponent', () => {
  let component: ZonesEntryListComponent;
  let fixture: ComponentFixture<ZonesEntryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ZonesEntryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ZonesEntryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
