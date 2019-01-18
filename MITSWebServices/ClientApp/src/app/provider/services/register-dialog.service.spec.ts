import { TestBed } from '@angular/core/testing';

import { RegisterDialogService } from './register-dialog.service';

describe('ProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RegisterDialogService = TestBed.get(RegisterDialogService);
    expect(service).toBeTruthy();
  });
});
