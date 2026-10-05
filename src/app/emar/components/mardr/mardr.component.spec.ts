import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MardrComponent } from './mardr.component';

describe('MardrComponent', () => {
  let component: MardrComponent;
  let fixture: ComponentFixture<MardrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MardrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MardrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
