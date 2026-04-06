import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxBreadcrumb } from './box-breadcrumb';

describe('BoxBreadcrumb', () => {
  let component: BoxBreadcrumb;
  let fixture: ComponentFixture<BoxBreadcrumb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxBreadcrumb]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxBreadcrumb);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
