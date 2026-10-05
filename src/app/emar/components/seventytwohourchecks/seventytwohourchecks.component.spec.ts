import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeventytwohourchecksComponent } from './seventytwohourchecks.component';

describe('SeventytwohourchecksComponent', () => {
  let component: SeventytwohourchecksComponent;
  let fixture: ComponentFixture<SeventytwohourchecksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeventytwohourchecksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeventytwohourchecksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
