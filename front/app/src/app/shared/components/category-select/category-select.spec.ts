import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategorySelect } from './category-select';

describe('CategorySelect', () => {
  let component: CategorySelect;
  let fixture: ComponentFixture<CategorySelect>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategorySelect]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategorySelect);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
