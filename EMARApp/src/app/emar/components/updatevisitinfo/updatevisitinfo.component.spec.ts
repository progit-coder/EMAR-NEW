import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdatevisitinfoComponent } from './updatevisitinfo.component';

describe('UpdatevisitinfoComponent', () => {
  let component: UpdatevisitinfoComponent;
  let fixture: ComponentFixture<UpdatevisitinfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdatevisitinfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdatevisitinfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
