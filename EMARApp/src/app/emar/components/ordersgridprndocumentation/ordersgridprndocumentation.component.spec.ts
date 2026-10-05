import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersgridprndocumentationComponent } from './ordersgridprndocumentation.component';

describe('OrdersgridprndocumentationComponent', () => {
  let component: OrdersgridprndocumentationComponent;
  let fixture: ComponentFixture<OrdersgridprndocumentationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdersgridprndocumentationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersgridprndocumentationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
