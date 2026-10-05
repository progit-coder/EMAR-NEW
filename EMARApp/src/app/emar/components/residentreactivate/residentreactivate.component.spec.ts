import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResidentreactivateComponent } from './residentreactivate.component';

describe('ResidentreactivateComponent', () => {
  let component: ResidentreactivateComponent;
  let fixture: ComponentFixture<ResidentreactivateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResidentreactivateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResidentreactivateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
