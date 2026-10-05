import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientdiagnosishistorymodalComponent } from './patientdiagnosishistorymodal.component';

describe('PatientdiagnosishistorymodalComponent', () => {
  let component: PatientdiagnosishistorymodalComponent;
  let fixture: ComponentFixture<PatientdiagnosishistorymodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PatientdiagnosishistorymodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientdiagnosishistorymodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
