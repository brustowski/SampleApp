import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PipelineMassUploadComponent } from './pipeline-mass-upload.component';

describe('PipelineMassUploadComponent', () => {
  let component: PipelineMassUploadComponent;
  let fixture: ComponentFixture<PipelineMassUploadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PipelineMassUploadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PipelineMassUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
