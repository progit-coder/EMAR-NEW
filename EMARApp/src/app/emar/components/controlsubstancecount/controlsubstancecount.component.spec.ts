import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlsubstancecountComponent } from './controlsubstancecount.component';

describe('ControlsubstancecountComponent', () => {
  let component: ControlsubstancecountComponent;
  let fixture: ComponentFixture<ControlsubstancecountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlsubstancecountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlsubstancecountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
