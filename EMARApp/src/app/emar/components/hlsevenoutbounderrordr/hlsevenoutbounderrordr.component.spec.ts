import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HlsevenoutbounderrordrComponent } from './hlsevenoutbounderrordr.component';

describe('HlsevenoutbounderrordrComponent', () => {
  let component: HlsevenoutbounderrordrComponent;
  let fixture: ComponentFixture<HlsevenoutbounderrordrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HlsevenoutbounderrordrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HlsevenoutbounderrordrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
