import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingforreviewComponent } from './pendingforreview.component';

describe('PendingforreviewComponent', () => {
  let component: PendingforreviewComponent;
  let fixture: ComponentFixture<PendingforreviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PendingforreviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingforreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
