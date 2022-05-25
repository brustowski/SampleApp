import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IsfListComponent } from './isf-list.component';

describe('InbondListComponent', () => {
  let component: IsfListComponent;
  let fixture: ComponentFixture<IsfListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IsfListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IsfListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
