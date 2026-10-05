import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OutboundhlsegmentfielddisplayconfigComponent } from './outboundhlsegmentfielddisplayconfig.component';

describe('OutboundhlsegmentfielddisplayconfigComponent', () => {
  let component: OutboundhlsegmentfielddisplayconfigComponent;
  let fixture: ComponentFixture<OutboundhlsegmentfielddisplayconfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OutboundhlsegmentfielddisplayconfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutboundhlsegmentfielddisplayconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
