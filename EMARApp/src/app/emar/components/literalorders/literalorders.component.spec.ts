import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LiteralordersComponent } from './literalorders.component';

describe('LiteralordersComponent', () => {
  let component: LiteralordersComponent;
  let fixture: ComponentFixture<LiteralordersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LiteralordersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LiteralordersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
