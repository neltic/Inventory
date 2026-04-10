import { FormControl } from "@angular/forms";

export interface IItem {
  itemId: number;
  name: string;
  notes: string;
  categoryId: number;
  hasStock?: number;
  createdAt?: string | Date;
  updatedAt?: string | Date;
}

export interface ItemForm {
  name: FormControl<string>;
  categoryId: FormControl<number>;
  notes: FormControl<string>;  
}

export interface IItemLocation {
  boxId: number;
  brandId: number;
  name: string;
  quantity: number;
}