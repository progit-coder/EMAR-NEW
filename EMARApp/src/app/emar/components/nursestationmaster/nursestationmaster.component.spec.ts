import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NursestationmasterComponent } from './nursestationmaster.component';

describe('NursestationmasterComponent', () => {
  let component: NursestationmasterComponent;
  let fixture: ComponentFixture<NursestationmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NursestationmasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NursestationmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
