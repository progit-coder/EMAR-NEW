import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FloorstockdrComponent } from './floorstockdr.component';

describe('FloorstockdrComponent', () => {
  let component: FloorstockdrComponent;
  let fixture: ComponentFixture<FloorstockdrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FloorstockdrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FloorstockdrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
