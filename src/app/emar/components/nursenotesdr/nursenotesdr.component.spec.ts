import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NursenotesdrComponent } from './nursenotesdr.component';

describe('NursenotesdrComponent', () => {
  let component: NursenotesdrComponent;
  let fixture: ComponentFixture<NursenotesdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NursenotesdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NursenotesdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
