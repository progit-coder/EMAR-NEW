import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommondashboardComponent } from './commondashboard.component';

describe('CommondashboardComponent', () => {
  let component: CommondashboardComponent;
  let fixture: ComponentFixture<CommondashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommondashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommondashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
