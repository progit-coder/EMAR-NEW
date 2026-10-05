import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhysciandetailsComponent } from './physciandetails.component';

describe('PhysciandetailsComponent', () => {
  let component: PhysciandetailsComponent;
  let fixture: ComponentFixture<PhysciandetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhysciandetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhysciandetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
