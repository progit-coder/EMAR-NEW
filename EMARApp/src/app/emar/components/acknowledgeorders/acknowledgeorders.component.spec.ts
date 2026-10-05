import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcknowledgeordersComponent } from './acknowledgeorders.component';

describe('AcknowledgeordersComponent', () => {
  let component: AcknowledgeordersComponent;
  let fixture: ComponentFixture<AcknowledgeordersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcknowledgeordersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcknowledgeordersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
