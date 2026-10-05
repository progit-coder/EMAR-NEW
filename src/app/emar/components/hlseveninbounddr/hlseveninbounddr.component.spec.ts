import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HlseveninbounddrComponent } from './hlseveninbounddr.component';

describe('HlseveninbounddrComponent', () => {
  let component: HlseveninbounddrComponent;
  let fixture: ComponentFixture<HlseveninbounddrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HlseveninbounddrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HlseveninbounddrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
