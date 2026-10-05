import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MailindoxComponent } from './mailindox.component';

describe('MailindoxComponent', () => {
  let component: MailindoxComponent;
  let fixture: ComponentFixture<MailindoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MailindoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MailindoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
