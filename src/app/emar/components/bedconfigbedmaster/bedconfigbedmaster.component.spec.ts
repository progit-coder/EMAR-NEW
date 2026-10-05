import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedconfigbedmasterComponent } from './bedconfigbedmaster.component';

describe('BedconfigbedmasterComponent', () => {
  let component: BedconfigbedmasterComponent;
  let fixture: ComponentFixture<BedconfigbedmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedconfigbedmasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedconfigbedmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
