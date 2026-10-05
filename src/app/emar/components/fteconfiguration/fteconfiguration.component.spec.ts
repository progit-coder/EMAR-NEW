import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FteconfigurationComponent } from './fteconfiguration.component';

describe('FteconfigurationComponent', () => {
  let component: FteconfigurationComponent;
  let fixture: ComponentFixture<FteconfigurationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FteconfigurationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FteconfigurationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
