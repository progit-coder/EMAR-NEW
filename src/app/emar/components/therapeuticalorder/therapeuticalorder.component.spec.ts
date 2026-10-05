import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TherapeuticalorderComponent } from './therapeuticalorder.component';

describe('TherapeuticalorderComponent', () => {
  let component: TherapeuticalorderComponent;
  let fixture: ComponentFixture<TherapeuticalorderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TherapeuticalorderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TherapeuticalorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
