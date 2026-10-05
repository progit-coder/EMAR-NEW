import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientallergyhistorymodalComponent } from './patientallergyhistorymodal.component';

describe('PatientallergyhistorymodalComponent', () => {
  let component: PatientallergyhistorymodalComponent;
  let fixture: ComponentFixture<PatientallergyhistorymodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PatientallergyhistorymodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientallergyhistorymodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
