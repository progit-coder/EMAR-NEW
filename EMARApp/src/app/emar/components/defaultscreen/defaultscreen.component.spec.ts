import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultscreenComponent } from './defaultscreen.component';

describe('DefaultscreenComponent', () => {
  let component: DefaultscreenComponent;
  let fixture: ComponentFixture<DefaultscreenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultscreenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultscreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
