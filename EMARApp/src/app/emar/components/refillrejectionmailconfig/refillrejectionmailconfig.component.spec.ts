import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RefillrejectionmailconfigComponent } from './refillrejectionmailconfig.component';

describe('RefillrejectionmailconfigComponent', () => {
  let component: RefillrejectionmailconfigComponent;
  let fixture: ComponentFixture<RefillrejectionmailconfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RefillrejectionmailconfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RefillrejectionmailconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
