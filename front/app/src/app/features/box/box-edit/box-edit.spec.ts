import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxEdit } from './box-edit';

describe('BoxEdit', () => {
  let component: BoxEdit;
  let fixture: ComponentFixture<BoxEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxEdit);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
