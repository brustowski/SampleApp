import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileUploadValidationResultComponent } from './file-upload-validation-result.component';

describe('FileUploadDetailedResultComponent', () => {
  let component: FileUploadValidationResultComponent;
  let fixture: ComponentFixture<FileUploadValidationResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileUploadValidationResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileUploadValidationResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
