import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonFileUploaderComponent } from './button-file-uploader.component';

describe('ButtonUploadFileComponent', () => {
  let component: ButtonFileUploaderComponent;
  let fixture: ComponentFixture<ButtonFileUploaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ButtonFileUploaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ButtonFileUploaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
