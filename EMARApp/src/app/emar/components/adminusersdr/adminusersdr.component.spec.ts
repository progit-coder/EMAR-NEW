import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminusersdrComponent } from './adminusersdr.component';

describe('AdminusersdrComponent', () => {
  let component: AdminusersdrComponent;
  let fixture: ComponentFixture<AdminusersdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminusersdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminusersdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
