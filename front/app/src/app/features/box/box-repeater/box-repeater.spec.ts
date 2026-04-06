import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxRepeater } from './box-repeater';

describe('BoxRepeater', () => {
  let component: BoxRepeater;
  let fixture: ComponentFixture<BoxRepeater>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxRepeater]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxRepeater);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
