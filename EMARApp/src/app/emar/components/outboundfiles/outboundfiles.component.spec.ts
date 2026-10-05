import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OutboundfilesComponent } from './outboundfiles.component';

describe('OutboundfilesComponent', () => {
  let component: OutboundfilesComponent;
  let fixture: ComponentFixture<OutboundfilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OutboundfilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutboundfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
