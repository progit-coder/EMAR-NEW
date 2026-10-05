import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RejectedrefillsComponent } from './rejectedrefills.component';

describe('RejectedrefillsComponent', () => {
  let component: RejectedrefillsComponent;
  let fixture: ComponentFixture<RejectedrefillsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RejectedrefillsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RejectedrefillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
