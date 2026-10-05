import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EkitComponent } from './ekit.component';

describe('EkitComponent', () => {
  let component: EkitComponent;
  let fixture: ComponentFixture<EkitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EkitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EkitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
