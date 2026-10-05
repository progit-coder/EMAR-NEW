import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilecertifiedordersdrComponent } from './profilecertifiedordersdr.component';

describe('ProfilecertifiedordersdrComponent', () => {
  let component: ProfilecertifiedordersdrComponent;
  let fixture: ComponentFixture<ProfilecertifiedordersdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfilecertifiedordersdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfilecertifiedordersdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
