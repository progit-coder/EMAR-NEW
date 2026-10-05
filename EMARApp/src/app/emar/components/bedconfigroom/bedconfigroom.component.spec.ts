import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedconfigroomComponent } from './bedconfigroom.component';

describe('BedconfigroomComponent', () => {
  let component: BedconfigroomComponent;
  let fixture: ComponentFixture<BedconfigroomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedconfigroomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedconfigroomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
