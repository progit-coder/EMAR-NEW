import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdergridComponent } from './ordergrid.component';

describe('OrdergridComponent', () => {
  let component: OrdergridComponent;
  let fixture: ComponentFixture<OrdergridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdergridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdergridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
