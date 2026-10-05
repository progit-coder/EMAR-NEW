import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EkitmedscheckinComponent } from './ekitmedscheckin.component';

describe('EkitmedscheckinComponent', () => {
  let component: EkitmedscheckinComponent;
  let fixture: ComponentFixture<EkitmedscheckinComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EkitmedscheckinComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EkitmedscheckinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
