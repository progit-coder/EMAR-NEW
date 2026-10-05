import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonmodalComponent } from './commonmodal.component';

describe('CommonmodalComponent', () => {
  let component: CommonmodalComponent;
  let fixture: ComponentFixture<CommonmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommonmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommonmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
