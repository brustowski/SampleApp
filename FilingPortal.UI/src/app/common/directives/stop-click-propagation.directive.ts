import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[lxftStopClickPropagation]'
})
export class StopClickPropagationDirective {

  @HostListener('click', ['$event'])
  public onClick(event: MouseEvent): void {
    event.stopPropagation();
  }

}
