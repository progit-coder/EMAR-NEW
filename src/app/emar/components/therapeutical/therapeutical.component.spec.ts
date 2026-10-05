import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TherapeuticalComponent } from './therapeutical.component';

describe('TherapeuticalComponent', () => {
  let component: TherapeuticalComponent;
  let fixture: ComponentFixture<TherapeuticalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TherapeuticalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TherapeuticalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
