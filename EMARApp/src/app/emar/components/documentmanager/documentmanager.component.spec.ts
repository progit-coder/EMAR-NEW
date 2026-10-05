import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentmanagerComponent } from './documentmanager.component';

describe('DocumentmanagerComponent', () => {
  let component: DocumentmanagerComponent;
  let fixture: ComponentFixture<DocumentmanagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentmanagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentmanagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
