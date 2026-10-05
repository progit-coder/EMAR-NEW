import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DemographiceditmodalComponent } from './demographiceditmodal.component';

describe('DemographiceditmodalComponent', () => {
  let component: DemographiceditmodalComponent;
  let fixture: ComponentFixture<DemographiceditmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DemographiceditmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DemographiceditmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
