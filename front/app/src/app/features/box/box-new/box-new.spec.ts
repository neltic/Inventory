import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxNew } from './box-new';

describe('BoxNew', () => {
  let component: BoxNew;
  let fixture: ComponentFixture<BoxNew>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxNew]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxNew);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
