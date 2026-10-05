import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleconfigComponent } from './roleconfig.component';

describe('RoleconfigComponent', () => {
  let component: RoleconfigComponent;
  let fixture: ComponentFixture<RoleconfigComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoleconfigComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleconfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
