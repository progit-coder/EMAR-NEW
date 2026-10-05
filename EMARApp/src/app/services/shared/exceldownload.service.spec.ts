import { TestBed, inject } from '@angular/core/testing';

import { ExceldownloadService } from './exceldownload.service';

describe('ExceldownloadService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ExceldownloadService]
    });
  });

  it('should be created', inject([ExceldownloadService], (service: ExceldownloadService) => {
    expect(service).toBeTruthy();
  }));
});
