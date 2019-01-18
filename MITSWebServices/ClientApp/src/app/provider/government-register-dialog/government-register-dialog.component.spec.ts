import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GovernmentRegisterDialogComponent } from './government-register-dialog.component';

describe('GovernmentRegisterDialogComponent', () => {
  let component: GovernmentRegisterDialogComponent;
  let fixture: ComponentFixture<GovernmentRegisterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GovernmentRegisterDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GovernmentRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
