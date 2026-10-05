import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PsychiatricdrComponent } from './psychiatricdr.component';

describe('PsychiatricdrComponent', () => {
  let component: PsychiatricdrComponent;
  let fixture: ComponentFixture<PsychiatricdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PsychiatricdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PsychiatricdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
