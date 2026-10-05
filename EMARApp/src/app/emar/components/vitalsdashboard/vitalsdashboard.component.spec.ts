import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VitalsdashboardComponent } from './vitalsdashboard.component';

describe('VitalsdashboardComponent', () => {
  let component: VitalsdashboardComponent;
  let fixture: ComponentFixture<VitalsdashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VitalsdashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VitalsdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
