import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckinmedsComponent } from './checkinmeds.component';

describe('CheckinmedsComponent', () => {
  let component: CheckinmedsComponent;
  let fixture: ComponentFixture<CheckinmedsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckinmedsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckinmedsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
