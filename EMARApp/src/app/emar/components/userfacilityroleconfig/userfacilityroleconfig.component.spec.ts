import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserfacilityroleconfigComponent } from './userfacilityroleconfig.component';

describe('UserfacilityroleconfigComponent', () => {
  let component: UserfacilityroleconfigComponent;
  let fixture: ComponentFixture<UserfacilityroleconfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserfacilityroleconfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserfacilityroleconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
