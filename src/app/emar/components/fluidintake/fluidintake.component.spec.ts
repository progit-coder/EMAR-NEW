import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FluidintakeComponent } from './fluidintake.component';

describe('FluidintakeComponent', () => {
  let component: FluidintakeComponent;
  let fixture: ComponentFixture<FluidintakeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FluidintakeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FluidintakeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
