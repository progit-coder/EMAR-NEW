import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VistinfoeditmodalComponent } from './vistinfoeditmodal.component';

describe('VistinfoeditmodalComponent', () => {
  let component: VistinfoeditmodalComponent;
  let fixture: ComponentFixture<VistinfoeditmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VistinfoeditmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VistinfoeditmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
