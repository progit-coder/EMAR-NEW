import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicationcheckindrComponent } from './medicationcheckindr.component';

describe('MedicationcheckindrComponent', () => {
  let component: MedicationcheckindrComponent;
  let fixture: ComponentFixture<MedicationcheckindrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MedicationcheckindrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MedicationcheckindrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
