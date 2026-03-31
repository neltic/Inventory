import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrandEditDialog } from './brand-edit-dialog';

describe('BrandEditDialog', () => {
  let component: BrandEditDialog;
  let fixture: ComponentFixture<BrandEditDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BrandEditDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BrandEditDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
