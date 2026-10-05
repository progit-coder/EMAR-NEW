import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrndocumentationComponent } from './prndocumentation.component';

describe('PrndocumentationComponent', () => {
  let component: PrndocumentationComponent;
  let fixture: ComponentFixture<PrndocumentationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrndocumentationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrndocumentationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
