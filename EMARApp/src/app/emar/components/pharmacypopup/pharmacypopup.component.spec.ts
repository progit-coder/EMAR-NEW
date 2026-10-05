import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PharmacypopupComponent } from './pharmacypopup.component';

describe('PharmacypopupComponent', () => {
  let component: PharmacypopupComponent;
  let fixture: ComponentFixture<PharmacypopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PharmacypopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PharmacypopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
