import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanytobedmapingComponent } from './companytobedmaping.component';

describe('CompanytobedmapingComponent', () => {
  let component: CompanytobedmapingComponent;
  let fixture: ComponentFixture<CompanytobedmapingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanytobedmapingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanytobedmapingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
