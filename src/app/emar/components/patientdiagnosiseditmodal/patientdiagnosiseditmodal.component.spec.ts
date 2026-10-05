import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientdiagnosiseditmodalComponent } from './patientdiagnosiseditmodal.component';

describe('PatientdiagnosiseditmodalComponent', () => {
  let component: PatientdiagnosiseditmodalComponent;
  let fixture: ComponentFixture<PatientdiagnosiseditmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PatientdiagnosiseditmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientdiagnosiseditmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
