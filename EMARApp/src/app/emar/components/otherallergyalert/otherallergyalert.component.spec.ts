import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherallergyalertComponent } from './otherallergyalert.component';

describe('OtherallergyalertComponent', () => {
  let component: OtherallergyalertComponent;
  let fixture: ComponentFixture<OtherallergyalertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtherallergyalertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtherallergyalertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
