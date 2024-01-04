import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RDFormComponent } from './rd-form.component';

describe('RDFormComponent', () => {
  let component: RDFormComponent;
  let fixture: ComponentFixture<RDFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RDFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RDFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
