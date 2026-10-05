import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DischargeresidentmodalComponent } from './dischargeresidentmodal.component';

describe('DischargeresidentmodalComponent', () => {
  let component: DischargeresidentmodalComponent;
  let fixture: ComponentFixture<DischargeresidentmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DischargeresidentmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DischargeresidentmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
