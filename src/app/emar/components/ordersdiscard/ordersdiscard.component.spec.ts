import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersdiscardComponent } from './ordersdiscard.component';

describe('OrdersdiscardComponent', () => {
  let component: OrdersdiscardComponent;
  let fixture: ComponentFixture<OrdersdiscardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdersdiscardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersdiscardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
