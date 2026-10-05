import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdercontrolsignoffdrComponent } from './ordercontrolsignoffdr.component';

describe('OrdercontrolsignoffdrComponent', () => {
  let component: OrdercontrolsignoffdrComponent;
  let fixture: ComponentFixture<OrdercontrolsignoffdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdercontrolsignoffdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdercontrolsignoffdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
