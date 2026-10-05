import { TestBed } from '@angular/core/testing';

import { AppGlobalErrorhandlerService } from './app-global-errorhandler.service';

describe('AppGlobalErrorhandlerService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AppGlobalErrorhandlerService = TestBed.get(AppGlobalErrorhandlerService);
    expect(service).toBeTruthy();
  });
});
