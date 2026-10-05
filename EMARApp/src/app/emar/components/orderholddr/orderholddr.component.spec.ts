import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderholddrComponent } from './orderholddr.component';

describe('OrderholddrComponent', () => {
  let component: OrderholddrComponent;
  let fixture: ComponentFixture<OrderholddrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderholddrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderholddrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
