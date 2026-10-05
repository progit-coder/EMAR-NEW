import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MedqtyonhandupdatedrComponent } from './medqtyonhandupdatedr.component';

describe('MedqtyonhandupdatedrComponent', () => {
  let component: MedqtyonhandupdatedrComponent;
  let fixture: ComponentFixture<MedqtyonhandupdatedrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MedqtyonhandupdatedrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MedqtyonhandupdatedrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
