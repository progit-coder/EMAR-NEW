import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientallergyeditmodalComponent } from './patientallergyeditmodal.component';

describe('PatientallergyeditmodalComponent', () => {
  let component: PatientallergyeditmodalComponent;
  let fixture: ComponentFixture<PatientallergyeditmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PatientallergyeditmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientallergyeditmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
