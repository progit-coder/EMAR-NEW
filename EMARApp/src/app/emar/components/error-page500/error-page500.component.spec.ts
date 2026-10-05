import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorPage500Component } from './error-page500.component';

describe('ErrorPage500Component', () => {
  let component: ErrorPage500Component;
  let fixture: ComponentFixture<ErrorPage500Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorPage500Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorPage500Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
