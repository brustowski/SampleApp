import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileUploadResultComponent } from './file-upload-result.component';

describe('FileUploadResultComponent', () => {
  let component: FileUploadResultComponent;
  let fixture: ComponentFixture<FileUploadResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileUploadResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileUploadResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
