import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommitteeRegisterDialogComponent } from './committee-register-dialog.component';

describe('CommitteeRegisterDialogComponent', () => {
  let component: CommitteeRegisterDialogComponent;
  let fixture: ComponentFixture<CommitteeRegisterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommitteeRegisterDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommitteeRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
