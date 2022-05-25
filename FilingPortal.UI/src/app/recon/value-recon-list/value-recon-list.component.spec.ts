import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ValueReconListComponent } from '.';

describe('ReconListComponent', () => {
  let component: ValueReconListComponent;
  let fixture: ComponentFixture<ValueReconListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValueReconListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValueReconListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
