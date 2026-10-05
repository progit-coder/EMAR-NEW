import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminapprovalComponent } from './adminapproval.component';

describe('AdminapprovalComponent', () => {
  let component: AdminapprovalComponent;
  let fixture: ComponentFixture<AdminapprovalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminapprovalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
