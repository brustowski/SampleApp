import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';

import { FieldNumberComponent } from './field-number.component';
import {By} from '@angular/platform-browser';
import {DebugElement} from '@angular/core';

describe('FieldNumberComponent', () => {
  let component: FieldNumberComponent;
  let fixture: ComponentFixture<FieldNumberComponent>;
  let debugElement: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldNumberComponent ],
      imports: [FormsModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldNumberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should check format "12334" as valid', () => {
    component.value = '12334';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(0);
  });

  it('should check format "123.34" as valid', () => {
    component.value = '123.34';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(0);
  });

  it('should check format "123.34.768" as invalid', () => {
    component.value = '123.34.768';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Number (decimals are supported) values are allowed');
  });

  it('should check format "aaaa" as invalid', () => {
    component.value = 'aaa';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Number (decimals are supported) values are allowed');
  });

  it('should check format "123aaaa" as invalid', () => {
    component.value = '123aaa';

    component.ngOnInit();

    expect(component.options.errors.length).toEqual(1);
    expect(component.options.errors[0]).toEqual('Incorrect value. Only Number (decimals are supported) values are allowed');
  });

  it('should be valid when mandatory and filled', () => {
    component.value = '123.56';
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
