import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColumnManagementButtonComponent } from './column-management-button.component';

describe('ColumnManagementButtonComponent', () => {
  let component: ColumnManagementButtonComponent;
  let fixture: ComponentFixture<ColumnManagementButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColumnManagementButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColumnManagementButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
