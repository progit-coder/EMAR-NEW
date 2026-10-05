import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InboundfilesComponent } from './inboundfiles.component';

describe('InboundfilesComponent', () => {
  let component: InboundfilesComponent;
  let fixture: ComponentFixture<InboundfilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InboundfilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InboundfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
