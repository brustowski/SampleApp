import { Component, OnInit } from '@angular/core';
import { LoaderService } from '@common/services';

@Component({
    selector: 'lxft-loader',
    templateUrl: './loader.component.html'
})
export class LoaderComponent implements OnInit {

    showLoader: boolean;

    constructor(private loaderService: LoaderService) { }

    ngOnInit() {
        this.loaderService.loaderDisplayed$.subscribe(x => this.showLoader = x);
    }
}
