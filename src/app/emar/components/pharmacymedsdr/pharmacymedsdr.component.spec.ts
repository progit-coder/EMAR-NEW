import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PharmacymedsdrComponent } from './pharmacymedsdr.component';

describe('PharmacymedsdrComponent', () => {
  let component: PharmacymedsdrComponent;
  let fixture: ComponentFixture<PharmacymedsdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PharmacymedsdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PharmacymedsdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
