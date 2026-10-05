import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderwithfavouritiesdrComponent } from './orderwithfavouritiesdr.component';

describe('OrderwithfavouritiesdrComponent', () => {
  let component: OrderwithfavouritiesdrComponent;
  let fixture: ComponentFixture<OrderwithfavouritiesdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderwithfavouritiesdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderwithfavouritiesdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
