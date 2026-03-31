import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrandSelect } from './brand-select';

describe('BrandSelect', () => {
  let component: BrandSelect;
  let fixture: ComponentFixture<BrandSelect>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BrandSelect]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BrandSelect);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
