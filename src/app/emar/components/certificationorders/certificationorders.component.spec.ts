import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CertificationordersComponent } from './certificationorders.component';

describe('CertificationordersComponent', () => {
  let component: CertificationordersComponent;
  let fixture: ComponentFixture<CertificationordersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CertificationordersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CertificationordersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
