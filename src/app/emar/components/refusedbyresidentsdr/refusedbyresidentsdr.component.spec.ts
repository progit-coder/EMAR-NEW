import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RefusedbyresidentsdrComponent } from './refusedbyresidentsdr.component';

describe('RefusedbyresidentsdrComponent', () => {
  let component: RefusedbyresidentsdrComponent;
  let fixture: ComponentFixture<RefusedbyresidentsdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RefusedbyresidentsdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RefusedbyresidentsdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
