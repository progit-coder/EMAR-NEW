import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CertifiedordersdrComponent } from './certifiedordersdr.component';

describe('CertifiedordersdrComponent', () => {
  let component: CertifiedordersdrComponent;
  let fixture: ComponentFixture<CertifiedordersdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CertifiedordersdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CertifiedordersdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
