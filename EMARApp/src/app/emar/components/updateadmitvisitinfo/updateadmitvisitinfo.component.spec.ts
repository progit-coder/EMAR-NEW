import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateadmitvisitinfoComponent } from './updateadmitvisitinfo.component';

describe('UpdateadmitvisitinfoComponent', () => {
  let component: UpdateadmitvisitinfoComponent;
  let fixture: ComponentFixture<UpdateadmitvisitinfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateadmitvisitinfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateadmitvisitinfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
