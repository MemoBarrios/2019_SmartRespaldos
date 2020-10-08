import { TestBed, inject } from '@angular/core/testing';

import { FechaAdapterService } from './fecha-adapter.service';

describe('FechaAdapterService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FechaAdapterService]
    });
  });

  it('should be created', inject([FechaAdapterService], (service: FechaAdapterService) => {
    expect(service).toBeTruthy();
  }));
});
