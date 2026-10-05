import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RemovemarmodalComponent } from './removemarmodal.component';

describe('RemovemarmodalComponent', () => {
  let component: RemovemarmodalComponent;
  let fixture: ComponentFixture<RemovemarmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RemovemarmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RemovemarmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
