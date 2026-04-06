import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemEdit } from './Item-edit';

describe('ItemEdit', () => {
  let component: ItemEdit;
  let fixture: ComponentFixture<ItemEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemEdit);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
