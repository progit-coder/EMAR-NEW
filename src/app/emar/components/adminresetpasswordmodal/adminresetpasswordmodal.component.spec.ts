import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminresetpasswordmodalComponent } from './adminresetpasswordmodal.component';

describe('AdminresetpasswordmodalComponent', () => {
  let component: AdminresetpasswordmodalComponent;
  let fixture: ComponentFixture<AdminresetpasswordmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminresetpasswordmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminresetpasswordmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
