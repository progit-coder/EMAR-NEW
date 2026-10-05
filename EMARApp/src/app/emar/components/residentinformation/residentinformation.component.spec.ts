import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResidentinformationComponent } from './residentinformation.component';

describe('ResidentinformationComponent', () => {
  let component: ResidentinformationComponent;
  let fixture: ComponentFixture<ResidentinformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResidentinformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResidentinformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
