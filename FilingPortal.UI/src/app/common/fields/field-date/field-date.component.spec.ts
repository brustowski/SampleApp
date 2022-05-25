import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';

import { FieldDateComponent } from './field-date.component';
import {By} from '@angular/platform-browser';
import {DebugElement} from '@angular/core';

describe('FieldDateComponent', () => {
  let component: FieldDateComponent;
  let fixture: ComponentFixture<FieldDateComponent>;
  let debugElement: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldDateComponent ],
      imports: [FormsModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldDateComponent);
    component = fixture.componentInstance;
    debugElement = fixture.debugElement;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should check format "12/31/2018" as valid', () => {
    component.value = '12/31/2018';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(0);
  });

  it('should check format "13/31/2018" as invalid', () => {
    component.value = '13/31/2018';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Date (mm/dd/yyyy) values are allowed');
  });

  it('should check format "12/32/2018" as invalid', () => {
    component.value = '12/32/2018';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Date (mm/dd/yyyy) values are allowed');
  });

  it('should check format "wrong" as invalid', () => {
    component.value = 'wrong';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Date (mm/dd/yyyy) values are allowed');
  });

  it('should check format "13/09/2018" as invalid', () => {
    component.value = '13/09/2018';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Date (mm/dd/yyyy) values are allowed');
  });

  it('should check format "02/29/2018" for not leap year as invalid', () => {
    component.value = '02/29/2018';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Date (mm/dd/yyyy) values are allowed');
  });

  it('should check format "02/29/2016" for leap year as valid', () => {
    component.value = '02/29/2016';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(0);
  });

  it('should check format "02/30/2016" for leap year as invalid', () => {
    component.value = '02/30/2016';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Date (mm/dd/yyyy) values are allowed');
  });

  it('should be valid when mandatory and filled', () => {
    component.value = '11/24/2018';
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
