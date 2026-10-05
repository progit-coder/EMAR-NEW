import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlsubstanceComponent } from './controlsubstance.component';

describe('ControlsubstanceComponent', () => {
  let component: ControlsubstanceComponent;
  let fixture: ComponentFixture<ControlsubstanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlsubstanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlsubstanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
