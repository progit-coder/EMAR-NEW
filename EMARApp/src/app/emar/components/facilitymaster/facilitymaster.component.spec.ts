import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FacilitymasterComponent } from './facilitymaster.component';

describe('FacilitymasterComponent', () => {
  let component: FacilitymasterComponent;
  let fixture: ComponentFixture<FacilitymasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FacilitymasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FacilitymasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
