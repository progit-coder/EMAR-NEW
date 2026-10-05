import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoommasterComponent } from './roommaster.component';

describe('RoommasterComponent', () => {
  let component: RoommasterComponent;
  let fixture: ComponentFixture<RoommasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoommasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoommasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
