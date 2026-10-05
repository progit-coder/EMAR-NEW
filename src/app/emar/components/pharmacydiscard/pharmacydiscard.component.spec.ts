import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PharmacydiscardComponent } from './pharmacydiscard.component';

describe('PharmacydiscardComponent', () => {
  let component: PharmacydiscardComponent;
  let fixture: ComponentFixture<PharmacydiscardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PharmacydiscardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PharmacydiscardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
