import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedconfigfacilityComponent } from './bedconfigfacility.component';

describe('BedconfigfacilityComponent', () => {
  let component: BedconfigfacilityComponent;
  let fixture: ComponentFixture<BedconfigfacilityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedconfigfacilityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedconfigfacilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
