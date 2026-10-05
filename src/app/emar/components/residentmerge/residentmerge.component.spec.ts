import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResidentmergeComponent } from './residentmerge.component';

describe('ResidentmergeComponent', () => {
  let component: ResidentmergeComponent;
  let fixture: ComponentFixture<ResidentmergeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResidentmergeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResidentmergeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
