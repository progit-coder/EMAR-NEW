import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderdrComponent } from './orderdr.component';

describe('OrderdrComponent', () => {
  let component: OrderdrComponent;
  let fixture: ComponentFixture<OrderdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
