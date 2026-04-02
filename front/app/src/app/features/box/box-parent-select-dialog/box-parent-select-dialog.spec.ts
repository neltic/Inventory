import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxParentSelectDialog } from './box-parent-select-dialog';

describe('BoxParentSelectDialog', () => {
  let component: BoxParentSelectDialog;
  let fixture: ComponentFixture<BoxParentSelectDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxParentSelectDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxParentSelectDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
