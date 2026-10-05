import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchpharmacynameComponent } from './searchpharmacyname.component';

describe('SearchpharmacynameComponent', () => {
  let component: SearchpharmacynameComponent;
  let fixture: ComponentFixture<SearchpharmacynameComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchpharmacynameComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchpharmacynameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
