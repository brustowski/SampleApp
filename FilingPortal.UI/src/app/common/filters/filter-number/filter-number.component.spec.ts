import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterNumberComponent } from './filter-number.component';
import { Filter } from '../models';
import { FormsModule } from '@angular/forms';


describe('FilterNumberComponent', () => {
  let component: FilterNumberComponent;
  let fixture: ComponentFixture<FilterNumberComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [FilterNumberComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterNumberComponent);
    component = fixture.componentInstance;
    component.filter = new Filter();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });

  it('should have maxLength from provided filter', () => {
    let filter: Filter = new Filter();
    filter.maxLength = 5;
    component.filter = filter;
    fixture.detectChanges();

    const element: HTMLElement = fixture.nativeElement;
    const input = element.querySelector('input');
    expect(input.maxLength).toEqual(5);
  });

  it('should have format number directive', () => {
    let filter: Filter = new Filter();
    component.filter = filter;
    fixture.detectChanges();

    const element: HTMLElement = fixture.nativeElement;
    const input = element.querySelector('input[lxftFormatNumber]');
    expect(input).toBeDefined();
  });

  it('should have filter value as model', () => {
    let filter: Filter = new Filter();
    filter.value = 'test value';
    component.filter = filter;
    fixture.detectChanges();

    const element: HTMLElement = fixture.nativeElement;
    element.dispatchEvent(new Event('input'));
    fixture.detectChanges();
    fixture.whenStable().then(() => {
      expect(component.filter.value).toEqual('test value');
    });
  });
});
