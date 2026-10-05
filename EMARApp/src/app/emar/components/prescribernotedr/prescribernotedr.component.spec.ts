import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrescribernotedrComponent } from './prescribernotedr.component';

describe('PrescribernotedrComponent', () => {
  let component: PrescribernotedrComponent;
  let fixture: ComponentFixture<PrescribernotedrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrescribernotedrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrescribernotedrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
