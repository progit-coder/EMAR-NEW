import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeventytwocheckdrComponent } from './seventytwocheckdr.component';

describe('SeventytwocheckdrComponent', () => {
  let component: SeventytwocheckdrComponent;
  let fixture: ComponentFixture<SeventytwocheckdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeventytwocheckdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeventytwocheckdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
