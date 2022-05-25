import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddManifestModalComponent } from './add-manifest-modal.component';

describe('AddManifestModalComponent', () => {
  let component: AddManifestModalComponent;
  let fixture: ComponentFixture<AddManifestModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddManifestModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddManifestModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
