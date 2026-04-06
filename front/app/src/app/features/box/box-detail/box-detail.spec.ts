import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxDetail } from './box-detail';

describe('BoxDetail', () => {
  let component: BoxDetail;
  let fixture: ComponentFixture<BoxDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxDetail]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
