import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MailconfigComponent } from './mailconfig.component';

describe('MailconfigComponent', () => {
  let component: MailconfigComponent;
  let fixture: ComponentFixture<MailconfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MailconfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MailconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
