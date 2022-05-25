import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RailManifestDataComponent } from './rail-manifest-data.component';

describe('RailManifestDataComponent', () => {
  let component: RailManifestDataComponent;
  let fixture: ComponentFixture<RailManifestDataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RailManifestDataComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RailManifestDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
