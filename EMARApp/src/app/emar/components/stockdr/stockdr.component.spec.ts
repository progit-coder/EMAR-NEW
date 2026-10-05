import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockdrComponent } from './stockdr.component';

describe('StockdrComponent', () => {
  let component: StockdrComponent;
  let fixture: ComponentFixture<StockdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
