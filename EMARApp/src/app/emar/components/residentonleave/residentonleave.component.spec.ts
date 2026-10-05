import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResidentonleaveComponent } from './residentonleave.component';

describe('ResidentonleaveComponent', () => {
  let component: ResidentonleaveComponent;
  let fixture: ComponentFixture<ResidentonleaveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResidentonleaveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResidentonleaveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
