import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedconfigwingComponent } from './bedconfigwing.component';

describe('BedconfigwingComponent', () => {
  let component: BedconfigwingComponent;
  let fixture: ComponentFixture<BedconfigwingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedconfigwingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedconfigwingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
