import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressbarmodalComponent } from './progressbarmodal.component';

describe('ProgressbarmodalComponent', () => {
  let component: ProgressbarmodalComponent;
  let fixture: ComponentFixture<ProgressbarmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgressbarmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgressbarmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
