import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DrXmlComponent } from './dr-xml.component';

describe('DrXmlComponent', () => {
  let component: DrXmlComponent;
  let fixture: ComponentFixture<DrXmlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DrXmlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DrXmlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
