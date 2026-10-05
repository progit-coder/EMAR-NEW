import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VisitinfohistorymodalComponent } from './visitinfohistorymodal.component';

describe('VisitinfohistorymodalComponent', () => {
  let component: VisitinfohistorymodalComponent;
  let fixture: ComponentFixture<VisitinfohistorymodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VisitinfohistorymodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VisitinfohistorymodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
