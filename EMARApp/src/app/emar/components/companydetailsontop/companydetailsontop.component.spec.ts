import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanydetailsontopComponent } from './companydetailsontop.component';

describe('CompanydetailsontopComponent', () => {
  let component: CompanydetailsontopComponent;
  let fixture: ComponentFixture<CompanydetailsontopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanydetailsontopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanydetailsontopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
