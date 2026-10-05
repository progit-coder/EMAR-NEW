import { TestBed, inject } from '@angular/core/testing';

import { TreenodeService } from './treenode.service';

describe('TreenodeService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TreenodeService]
    });
  });

  it('should be created', inject([TreenodeService], (service: TreenodeService) => {
    expect(service).toBeTruthy();
  }));
});
