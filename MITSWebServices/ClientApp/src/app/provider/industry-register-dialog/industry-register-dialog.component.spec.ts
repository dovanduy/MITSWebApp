import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndustryRegisterDialogComponent } from './industry-register-dialog.component';

describe('IndustryRegisterDialogComponent', () => {
  let component: IndustryRegisterDialogComponent;
  let fixture: ComponentFixture<IndustryRegisterDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustryRegisterDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndustryRegisterDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
