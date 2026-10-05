import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BarcodedrComponent } from './barcodedr.component';

describe('BarcodedrComponent', () => {
  let component: BarcodedrComponent;
  let fixture: ComponentFixture<BarcodedrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BarcodedrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BarcodedrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
