import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BiometericdrComponent } from './biometericdr.component';

describe('BiometericdrComponent', () => {
  let component: BiometericdrComponent;
  let fixture: ComponentFixture<BiometericdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BiometericdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BiometericdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
