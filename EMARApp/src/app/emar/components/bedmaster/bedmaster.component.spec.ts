import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BedmasterComponent } from './bedmaster.component';

describe('BedmasterComponent', () => {
  let component: BedmasterComponent;
  let fixture: ComponentFixture<BedmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BedmasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BedmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
