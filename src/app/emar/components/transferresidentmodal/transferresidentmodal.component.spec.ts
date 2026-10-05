import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferresidentmodalComponent } from './transferresidentmodal.component';

describe('TransferresidentmodalComponent', () => {
  let component: TransferresidentmodalComponent;
  let fixture: ComponentFixture<TransferresidentmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferresidentmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferresidentmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
