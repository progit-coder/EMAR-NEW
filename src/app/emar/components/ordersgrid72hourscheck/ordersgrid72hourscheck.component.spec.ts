import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Ordersgrid72hourscheckComponent } from './ordersgrid72hourscheck.component';

describe('Ordersgrid72hourscheckComponent', () => {
  let component: Ordersgrid72hourscheckComponent;
  let fixture: ComponentFixture<Ordersgrid72hourscheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Ordersgrid72hourscheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Ordersgrid72hourscheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
