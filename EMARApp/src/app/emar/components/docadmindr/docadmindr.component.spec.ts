import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocadmindrComponent } from './docadmindr.component';

describe('DocadmindrComponent', () => {
  let component: DocadmindrComponent;
  let fixture: ComponentFixture<DocadmindrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocadmindrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocadmindrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
