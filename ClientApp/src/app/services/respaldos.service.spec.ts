import { TestBed, inject } from '@angular/core/testing';

import { RespaldosService } from './respaldos.service';

describe('RespaldosService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RespaldosService]
    });
  });

  it('should be created', inject([RespaldosService], (service: RespaldosService) => {
    expect(service).toBeTruthy();
  }));
});
