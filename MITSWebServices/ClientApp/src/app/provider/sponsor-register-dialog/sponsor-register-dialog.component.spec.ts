import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SponsorRegisterDialogComponent } from './sponsor-register-dialog.component';

describe('SponsorRegisterDialogComponent', () => {
  let component: SponsorRegisterDialogComponent;
  let fixture: ComponentFixture<SponsorRegisterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SponsorRegisterDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SponsorRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
