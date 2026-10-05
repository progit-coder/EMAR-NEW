import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UseractivitydrComponent } from './useractivitydr.component';

describe('UseractivitydrComponent', () => {
  let component: UseractivitydrComponent;
  let fixture: ComponentFixture<UseractivitydrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UseractivitydrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UseractivitydrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
