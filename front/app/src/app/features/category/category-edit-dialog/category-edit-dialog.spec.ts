import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryEditDialog } from './category-edit-dialog';

describe('CategoryEditDialog', () => {
  let component: CategoryEditDialog;
  let fixture: ComponentFixture<CategoryEditDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryEditDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryEditDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
