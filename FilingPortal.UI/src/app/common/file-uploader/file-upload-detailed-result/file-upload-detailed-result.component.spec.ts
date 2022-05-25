import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FileUploadDetailedResultComponent } from './file-upload-detailed-result.component';

describe('FileUploadDetailedResultComponent', () => {
  let component: FileUploadDetailedResultComponent;
  let fixture: ComponentFixture<FileUploadDetailedResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FileUploadDetailedResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FileUploadDetailedResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
