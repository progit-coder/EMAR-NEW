import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TodaysdatealertComponent } from './todaysdatealert.component';

describe('TodaysdatealertComponent', () => {
  let component: TodaysdatealertComponent;
  let fixture: ComponentFixture<TodaysdatealertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TodaysdatealertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TodaysdatealertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
