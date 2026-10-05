import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EkitdrComponent } from './ekitdr.component';

describe('EkitdrComponent', () => {
  let component: EkitdrComponent;
  let fixture: ComponentFixture<EkitdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EkitdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EkitdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
