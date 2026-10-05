import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmaruserComponent } from './emaruser.component';

describe('EmaruserComponent', () => {
  let component: EmaruserComponent;
  let fixture: ComponentFixture<EmaruserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmaruserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmaruserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
