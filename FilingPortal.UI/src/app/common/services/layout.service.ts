import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {

  constructor() { }

  getGridAvailableHeight(knownHeight: number = 24) {
    const mainSection = document;
    const header = mainSection ? mainSection.querySelectorAll('.header') : [];
    const filterSection = mainSection ? mainSection.querySelectorAll('.filters') : [];
    const navSection = mainSection ? mainSection.querySelectorAll('.nav-tabs') : [];
    const tableNav = mainSection ? mainSection.querySelectorAll('.toolbar-nav-section') : [];
    const tableButtons = mainSection ? mainSection.querySelectorAll('.grid-header') : [];
    const pageTitle = mainSection ? mainSection.querySelectorAll('.page-header') : [];
    let height = 2 + (knownHeight || 0); // grid borders

    // header
    if (header.length){
      height += header[0].offsetHeight;
    }

    // filters panel
    if (filterSection.length > 0) {
      height += filterSection[0].offsetHeight;
    }

    if (navSection.length > 0) {
      height += navSection[0].offsetHeight + 15;
    }

    // paging
    if (tableNav.length > 0) {
      height += tableNav[0].offsetHeight;
    }

    // toolbar
    if (tableButtons.length > 0) {
      height += tableButtons[0].offsetHeight;
    }

    // page title
    if (pageTitle.length > 0) {
      height += pageTitle[0].offsetHeight + 16;
    }

    // available space
    return window.innerHeight - height;
  }
}
