import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IcdmasterComponent } from './icdmaster.component';

describe('IcdmasterComponent', () => {
  let component: IcdmasterComponent;
  let fixture: ComponentFixture<IcdmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IcdmasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IcdmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
