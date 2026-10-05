import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanningbypassdrComponent } from './scanningbypassdr.component';

describe('ScanningbypassdrComponent', () => {
  let component: ScanningbypassdrComponent;
  let fixture: ComponentFixture<ScanningbypassdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScanningbypassdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScanningbypassdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
