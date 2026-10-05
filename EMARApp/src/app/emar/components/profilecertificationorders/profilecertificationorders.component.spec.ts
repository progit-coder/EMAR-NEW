import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilecertificationordersComponent } from './profilecertificationorders.component';

describe('ProfilecertificationordersComponent', () => {
  let component: ProfilecertificationordersComponent;
  let fixture: ComponentFixture<ProfilecertificationordersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfilecertificationordersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfilecertificationordersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
