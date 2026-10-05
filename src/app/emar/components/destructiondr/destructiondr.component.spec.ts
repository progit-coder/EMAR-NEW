import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DestructiondrComponent } from './destructiondr.component';

describe('DestructiondrComponent', () => {
  let component: DestructiondrComponent;
  let fixture: ComponentFixture<DestructiondrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DestructiondrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DestructiondrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
