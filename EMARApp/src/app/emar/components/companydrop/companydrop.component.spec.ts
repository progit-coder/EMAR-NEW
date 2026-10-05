import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanydropComponent } from './companydrop.component';

describe('CompanydropComponent', () => {
  let component: CompanydropComponent;
  let fixture: ComponentFixture<CompanydropComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanydropComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanydropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
