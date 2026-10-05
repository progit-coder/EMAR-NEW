import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FrequencymappingComponent } from './frequencymapping.component';

describe('FrequencymappingComponent', () => {
  let component: FrequencymappingComponent;
  let fixture: ComponentFixture<FrequencymappingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FrequencymappingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FrequencymappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
