import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersendingsoonComponent } from './ordersendingsoon.component';

describe('OrdersendingsoonComponent', () => {
  let component: OrdersendingsoonComponent;
  let fixture: ComponentFixture<OrdersendingsoonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdersendingsoonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersendingsoonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
