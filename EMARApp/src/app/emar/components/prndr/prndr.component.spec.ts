import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrndrComponent } from './prndr.component';

describe('PrndrComponent', () => {
  let component: PrndrComponent;
  let fixture: ComponentFixture<PrndrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrndrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrndrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
