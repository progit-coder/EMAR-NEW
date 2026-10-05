import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetupconfigdatadrComponent } from './setupconfigdatadr.component';

describe('SetupconfigdatadrComponent', () => {
  let component: SetupconfigdatadrComponent;
  let fixture: ComponentFixture<SetupconfigdatadrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetupconfigdatadrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetupconfigdatadrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
