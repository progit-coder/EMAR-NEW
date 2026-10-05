import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementsuserinputmasterComponent } from './measurementsuserinputmaster.component';

describe('MeasurementsuserinputmasterComponent', () => {
  let component: MeasurementsuserinputmasterComponent;
  let fixture: ComponentFixture<MeasurementsuserinputmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MeasurementsuserinputmasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MeasurementsuserinputmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
