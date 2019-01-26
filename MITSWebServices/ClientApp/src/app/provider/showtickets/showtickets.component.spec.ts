import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowticketsComponent } from './showtickets.component';

describe('ShowticketsComponent', () => {
  let component: ShowticketsComponent;
  let fixture: ComponentFixture<ShowticketsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowticketsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowticketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
