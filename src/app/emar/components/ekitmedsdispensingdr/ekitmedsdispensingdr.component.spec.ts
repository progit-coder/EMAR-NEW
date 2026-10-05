import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EkitmedsdispensingdrComponent } from './ekitmedsdispensingdr.component';

describe('EkitmedsdispensingdrComponent', () => {
  let component: EkitmedsdispensingdrComponent;
  let fixture: ComponentFixture<EkitmedsdispensingdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EkitmedsdispensingdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EkitmedsdispensingdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
