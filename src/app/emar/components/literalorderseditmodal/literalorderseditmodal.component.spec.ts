import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LiteralorderseditmodalComponent } from './literalorderseditmodal.component';

describe('LiteralorderseditmodalComponent', () => {
  let component: LiteralorderseditmodalComponent;
  let fixture: ComponentFixture<LiteralorderseditmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LiteralorderseditmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LiteralorderseditmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
