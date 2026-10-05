import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedconfigfloorComponent } from './bedconfigfloor.component';

describe('BedconfigfloorComponent', () => {
  let component: BedconfigfloorComponent;
  let fixture: ComponentFixture<BedconfigfloorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedconfigfloorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedconfigfloorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
