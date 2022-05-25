import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ZonesFtz214ListComponent } from './zones-ftz-214-list.component';

describe('InbondListComponent', () => {
  let component: ZonesFtz214ListComponent;
  let fixture: ComponentFixture<ZonesFtz214ListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ZonesFtz214ListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ZonesFtz214ListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
