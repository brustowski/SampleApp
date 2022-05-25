import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColumnManagementComponent } from './column-management.component';

describe('ColumnManagementComponent', () => {
  let component: ColumnManagementComponent;
  let fixture: ComponentFixture<ColumnManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColumnManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColumnManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
