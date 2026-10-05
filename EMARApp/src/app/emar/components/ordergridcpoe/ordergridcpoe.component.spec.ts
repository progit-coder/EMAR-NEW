import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdergridcpoeComponent } from './ordergridcpoe.component';

describe('OrdergridcpoeComponent', () => {
  let component: OrdergridcpoeComponent;
  let fixture: ComponentFixture<OrdergridcpoeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdergridcpoeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdergridcpoeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
