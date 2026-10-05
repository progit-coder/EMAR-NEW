import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EkitmedsexpirationdrComponent } from './ekitmedsexpirationdr.component';

describe('EkitmedsexpirationdrComponent', () => {
  let component: EkitmedsexpirationdrComponent;
  let fixture: ComponentFixture<EkitmedsexpirationdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EkitmedsexpirationdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EkitmedsexpirationdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
