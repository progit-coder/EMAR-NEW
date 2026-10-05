import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllergymasterComponent } from './allergymaster.component';

describe('AllergymasterComponent', () => {
  let component: AllergymasterComponent;
  let fixture: ComponentFixture<AllergymasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllergymasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllergymasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
