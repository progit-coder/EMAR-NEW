import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResidentgridComponent } from './residentgrid.component';

describe('ResidentgridComponent', () => {
  let component: ResidentgridComponent;
  let fixture: ComponentFixture<ResidentgridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResidentgridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResidentgridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
