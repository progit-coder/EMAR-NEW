import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NursenotesComponent } from './nursenotes.component';

describe('NursenotesComponent', () => {
  let component: NursenotesComponent;
  let fixture: ComponentFixture<NursenotesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NursenotesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NursenotesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
