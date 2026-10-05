import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdercontrolsubstancedrComponent } from './ordercontrolsubstancedr.component';

describe('OrdercontrolsubstancedrComponent', () => {
  let component: OrdercontrolsubstancedrComponent;
  let fixture: ComponentFixture<OrdercontrolsubstancedrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdercontrolsubstancedrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdercontrolsubstancedrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
