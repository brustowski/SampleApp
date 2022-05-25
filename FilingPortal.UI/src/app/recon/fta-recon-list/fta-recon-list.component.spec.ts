import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FtaReconListComponent } from '.';

describe('ReconListComponent', () => {
  let component: FtaReconListComponent;
  let fixture: ComponentFixture<FtaReconListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FtaReconListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FtaReconListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
