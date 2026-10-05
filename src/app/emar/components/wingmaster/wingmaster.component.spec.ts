import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WingmasterComponent } from './wingmaster.component';

describe('WingmasterComponent', () => {
  let component: WingmasterComponent;
  let fixture: ComponentFixture<WingmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WingmasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WingmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
