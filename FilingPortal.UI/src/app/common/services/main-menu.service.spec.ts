import { TestBed, inject } from '@angular/core/testing';

import { MainMenuService } from './main-menu.service';

describe('MainMenuServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MainMenuService]
    });
  });

  it('should be created', inject([MainMenuService], (service: MainMenuService) => {
    expect(service).toBeTruthy();
  }));
});
