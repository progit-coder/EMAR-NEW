import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RefilldrComponent } from './refilldr.component';

describe('RefilldrComponent', () => {
  let component: RefilldrComponent;
  let fixture: ComponentFixture<RefilldrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RefilldrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RefilldrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
