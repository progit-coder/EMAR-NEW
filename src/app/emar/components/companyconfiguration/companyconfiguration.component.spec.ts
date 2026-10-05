import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyconfigurationComponent } from './companyconfiguration.component';

describe('CompanyconfigurationComponent', () => {
  let component: CompanyconfigurationComponent;
  let fixture: ComponentFixture<CompanyconfigurationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyconfigurationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyconfigurationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
