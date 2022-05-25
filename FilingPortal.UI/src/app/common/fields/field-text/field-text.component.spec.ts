import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';

import { FieldTextComponent } from './field-text.component';
import {By} from '@angular/platform-browser';
import {DebugElement} from '@angular/core';

describe('FieldTextComponent', () => {
  let component: FieldTextComponent;
  let fixture: ComponentFixture<FieldTextComponent>;
  let debugElement: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldTextComponent ],
      imports: [FormsModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldTextComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

    it('should be valid when mandatory and filled', () => {
    component.value = 'some text';
    component.options.isMandatory = true;

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(0);
  });

  it('should be invalid when mandatory and not filled', () => {
    component.value = '';
    component.options.isMandatory = true;

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Field is required');
  });

  it('should be valid when not mandatory and not filled', () => {
    component.value = '';
    component.options.isMandatory = false;

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(0);
  });

  it('should be disabled when isDisabled flag is set to true', () => {
    component.options.isDisabled = true;
    fixture.detectChanges();

    fixture.whenStable().then(() => {
      expect(debugElement.query(By.css("input")).nativeElement.disabled).toBeTruthy();
    });
  });

  it('should not be disabled when isDisabled flag is set to false', () => {
    component.options.isDisabled = false;
    fixture.detectChanges();

    fixture.whenStable().then(() => {
      expect(debugElement.query(By.css("input")).nativeElement.disabled).toBeFalsy();
    });
  });
});
