import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DrfirstfilesComponent } from './drfirstfiles.component';

describe('DrfirstfilesComponent', () => {
  let component: DrfirstfilesComponent;
  let fixture: ComponentFixture<DrfirstfilesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DrfirstfilesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DrfirstfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
