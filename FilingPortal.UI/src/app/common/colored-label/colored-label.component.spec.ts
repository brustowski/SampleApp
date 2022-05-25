import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { ColoredLabelComponent } from "./colored-label.component";
import { DebugElement } from "@angular/core";
import {By} from "@angular/platform-browser";


describe("ColoredLabelComponent", () => {
  let fixture: ComponentFixture<ColoredLabelComponent>;
  let component: ColoredLabelComponent;
  let debugElement: DebugElement;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColoredLabelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColoredLabelComponent);
    component = fixture.componentInstance;
    debugElement = fixture.debugElement;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });

  it("shoud be empty if status not set", () => {
    const div: HTMLElement = debugElement.nativeElement;
    expect(div.childElementCount).toBe(0);
  });

  it("should contains 'open' class for 'Open' status", () => {
    component.title = "Open";
    component.css = "open";
    fixture.detectChanges();
    expect(debugElement.query(By.css(".status-label.open")).nativeElement.innerText).toBe("Open");
  });

  it("should contains 'ready' class for 'Ready' status", () => {
    component.title = "Ready";
    component.css = "ready";
    fixture.detectChanges();
    expect(debugElement.query(By.css(".status-label.ready")).nativeElement.innerText).toBe("Ready");
  });

  it("should contains 'saved' class for 'Saved' status", () => {
    component.title = "Saved";
    component.css = "saved";
    fixture.detectChanges();
    expect(debugElement.query(By.css(".status-label.saved")).nativeElement.innerText).toBe("Saved");
  });

  it("should contains 'mapped' class for 'Mapped' status", () => {
    component.title = "Mapped";
    component.css = "mapped";
    fixture.detectChanges();
    expect(debugElement.query(By.css(".status-label.mapped")).nativeElement.innerText).toBe("Mapped");
  });

  it("should contains 'filed' class for 'Filed' status", () => {
    component.title = "Filed";
    component.css = "filed";
    fixture.detectChanges();
    expect(debugElement.query(By.css(".status-label.filed")).nativeElement.innerText).toBe("Filed");
  });

  it("should contains 'inprogress' class for 'In Progress' status", () => {
    component.title = "In Progress";
    component.css = "inprogress";
    fixture.detectChanges();
    expect(debugElement.query(By.css(".status-label.inprogress")).nativeElement.innerText).toBe("In Progress");
  });

  it("should contains 'error' class for 'Error' status", () => {
    component.title = "Error";
    component.css = "error";
    fixture.detectChanges();
    expect(debugElement.query(By.css(".status-label.error")).nativeElement.innerText).toBe("Error");
  });
});
