import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FloormasterComponent } from './floormaster.component';

describe('FloormasterComponent', () => {
  let component: FloormasterComponent;
  let fixture: ComponentFixture<FloormasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FloormasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FloormasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
