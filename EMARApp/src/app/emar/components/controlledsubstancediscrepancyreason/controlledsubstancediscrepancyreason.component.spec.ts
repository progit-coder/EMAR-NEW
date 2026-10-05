import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlledsubstancediscrepancyreasonComponent } from './controlledsubstancediscrepancyreason.component';

describe('ControlledsubstancediscrepancyreasonComponent', () => {
  let component: ControlledsubstancediscrepancyreasonComponent;
  let fixture: ComponentFixture<ControlledsubstancediscrepancyreasonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlledsubstancediscrepancyreasonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlledsubstancediscrepancyreasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
