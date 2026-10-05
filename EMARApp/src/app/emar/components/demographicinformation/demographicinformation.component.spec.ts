import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DemographicinformationComponent } from './demographicinformation.component';

describe('DemographicinformationComponent', () => {
  let component: DemographicinformationComponent;
  let fixture: ComponentFixture<DemographicinformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DemographicinformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DemographicinformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
