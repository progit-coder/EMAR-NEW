import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanybedconfigComponent } from './companybedconfig.component';

describe('CompanybedconfigComponent', () => {
  let component: CompanybedconfigComponent;
  let fixture: ComponentFixture<CompanybedconfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanybedconfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanybedconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
