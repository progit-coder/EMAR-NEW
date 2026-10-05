import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HlsegmentfieldsconfigComponent } from './hlsegmentfieldsconfig.component';

describe('HlsegmentfieldsconfigComponent', () => {
  let component: HlsegmentfieldsconfigComponent;
  let fixture: ComponentFixture<HlsegmentfieldsconfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HlsegmentfieldsconfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HlsegmentfieldsconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
