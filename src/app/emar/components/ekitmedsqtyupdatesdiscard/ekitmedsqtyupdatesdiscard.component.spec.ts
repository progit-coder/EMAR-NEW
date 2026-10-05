import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EkitmedsqtyupdatesdiscardComponent } from './ekitmedsqtyupdatesdiscard.component';

describe('EkitmedsqtyupdatesdiscardComponent', () => {
  let component: EkitmedsqtyupdatesdiscardComponent;
  let fixture: ComponentFixture<EkitmedsqtyupdatesdiscardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EkitmedsqtyupdatesdiscardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EkitmedsqtyupdatesdiscardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
