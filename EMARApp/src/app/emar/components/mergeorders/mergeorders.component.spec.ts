import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MergeordersComponent } from './mergeorders.component';

describe('MergeordersComponent', () => {
  let component: MergeordersComponent;
  let fixture: ComponentFixture<MergeordersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MergeordersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MergeordersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
