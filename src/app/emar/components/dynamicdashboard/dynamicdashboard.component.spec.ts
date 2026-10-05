import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicdashboardComponent } from './dynamicdashboard.component';

describe('DynamicdashboardComponent', () => {
  let component: DynamicdashboardComponent;
  let fixture: ComponentFixture<DynamicdashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DynamicdashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DynamicdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
