import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResidentmedicationComponent } from './residentmedication.component';

describe('ResidentmedicationComponent', () => {
  let component: ResidentmedicationComponent;
  let fixture: ComponentFixture<ResidentmedicationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResidentmedicationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResidentmedicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
