import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderchangedrComponent } from './orderchangedr.component';

describe('OrderchangedrComponent', () => {
  let component: OrderchangedrComponent;
  let fixture: ComponentFixture<OrderchangedrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderchangedrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderchangedrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
