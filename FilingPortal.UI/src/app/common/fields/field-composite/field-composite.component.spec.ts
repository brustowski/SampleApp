import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldCompositeComponent } from './field-composite.component';

describe('FieldCompositeComponent', () => {
  let component: FieldCompositeComponent;
  let fixture: ComponentFixture<FieldCompositeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldCompositeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldCompositeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
