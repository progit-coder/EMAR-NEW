import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MedrefComponent } from './medref.component';

describe('MedrefComponent', () => {
  let component: MedrefComponent;
  let fixture: ComponentFixture<MedrefComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MedrefComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MedrefComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
