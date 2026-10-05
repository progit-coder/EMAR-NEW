import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentcheckedordersComponent } from './documentcheckedorders.component';

describe('DocumentcheckedordersComponent', () => {
  let component: DocumentcheckedordersComponent;
  let fixture: ComponentFixture<DocumentcheckedordersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentcheckedordersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentcheckedordersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
