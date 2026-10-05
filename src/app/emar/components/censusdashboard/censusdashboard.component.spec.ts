import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CensusdashboardComponent } from './censusdashboard.component';

describe('CensusdashboardComponent', () => {
  let component: CensusdashboardComponent;
  let fixture: ComponentFixture<CensusdashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CensusdashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CensusdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
