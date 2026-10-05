import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchDrugNameComponent } from './search-drug-name.component';

describe('SearchDrugNameComponent', () => {
  let component: SearchDrugNameComponent;
  let fixture: ComponentFixture<SearchDrugNameComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchDrugNameComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchDrugNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
