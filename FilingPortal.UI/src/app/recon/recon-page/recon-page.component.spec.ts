import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReconPageComponent } from './recon-page.component';

describe('AuditPageComponent', () => {
  let component: ReconPageComponent;
  let fixture: ComponentFixture<ReconPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReconPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReconPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
