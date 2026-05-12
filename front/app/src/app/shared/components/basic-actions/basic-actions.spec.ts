import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasicActions } from './basic-actions';

describe('BasicActions', () => {
  let component: BasicActions;
  let fixture: ComponentFixture<BasicActions>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BasicActions]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BasicActions);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
