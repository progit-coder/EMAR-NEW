import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedconfigcompanymasterComponent } from './bedconfigcompanymaster.component';

describe('BedconfigcompanymasterComponent', () => {
  let component: BedconfigcompanymasterComponent;
  let fixture: ComponentFixture<BedconfigcompanymasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedconfigcompanymasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedconfigcompanymasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
