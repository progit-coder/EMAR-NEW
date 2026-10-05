import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderinfocpoeComponent } from './orderinfocpoe.component';

describe('OrderinfocpoeComponent', () => {
  let component: OrderinfocpoeComponent;
  let fixture: ComponentFixture<OrderinfocpoeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderinfocpoeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderinfocpoeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
