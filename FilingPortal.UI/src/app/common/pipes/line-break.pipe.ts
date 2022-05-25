import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'lineBreak'
})
export class LineBreakPipe implements PipeTransform {

  transform(value: string, symbolsInLine: number): string[] {
    return value.match(new RegExp('.{1,' + symbolsInLine + '}', 'g'));
  }

}
