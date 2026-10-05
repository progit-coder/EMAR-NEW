import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DemographichistorymodalComponent } from './demographichistorymodal.component';

describe('DemographichistorymodalComponent', () => {
  let component: DemographichistorymodalComponent;
  let fixture: ComponentFixture<DemographichistorymodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DemographichistorymodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DemographichistorymodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
