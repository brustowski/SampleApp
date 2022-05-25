import { Directive, Input, ElementRef, HostListener, Renderer2, ViewContainerRef, ComponentFactoryResolver } from '@angular/core';
import { CopyToClipboardComponent } from '@common/copy-to-clipboard/copy-to-clipboard.component';

@Directive({
  selector: '[lxftCopyToClipboard]'
})
export class CopyToClipboardDirective {

  @Input() ctcValue: string;

  constructor(private renderer: Renderer2,
    private element: ElementRef,
    private componentFactoryResolver: ComponentFactoryResolver,
    private viewContainerRef: ViewContainerRef) { }

  private get DirectiveComponentId(): number {
    return 11001; // just a big random value for exact identifying component
  }

  @HostListener('mouseenter') onMouseEnter() {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(CopyToClipboardComponent);
    const component = this.viewContainerRef.createComponent(componentFactory, this.DirectiveComponentId);
    component.instance.ctcValue = this.ctcValue;

    const componentElement = component.location.nativeElement;
    this.renderer.appendChild(this.element.nativeElement, componentElement);
  }

  @HostListener('mouseleave') onMouseLeave() {
    this.viewContainerRef.remove(this.DirectiveComponentId);
  }
}
