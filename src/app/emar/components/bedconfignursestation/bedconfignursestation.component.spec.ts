import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedconfignursestationComponent } from './bedconfignursestation.component';

describe('BedconfignursestationComponent', () => {
  let component: BedconfignursestationComponent;
  let fixture: ComponentFixture<BedconfignursestationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedconfignursestationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedconfignursestationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
