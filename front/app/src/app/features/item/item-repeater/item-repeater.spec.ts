import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ItemRepeater } from './item-repeater';

describe('ItemRepeater', () => {
  let component: ItemRepeater;
  let fixture: ComponentFixture<ItemRepeater>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemRepeater]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemRepeater);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
