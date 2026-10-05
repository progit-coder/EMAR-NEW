import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PharmacymedsexpirydrComponent } from './pharmacymedsexpirydr.component';

describe('PharmacymedsexpirydrComponent', () => {
  let component: PharmacymedsexpirydrComponent;
  let fixture: ComponentFixture<PharmacymedsexpirydrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PharmacymedsexpirydrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PharmacymedsexpirydrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
