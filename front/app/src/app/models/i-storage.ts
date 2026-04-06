import { FormControl } from "@angular/forms";

export interface IItemInBox {
  itemId: number;
  name: string;
  brandId: number;
  categoryId: number;
  quantity: number;
  updatedAt?: string | Date;
}

export interface IItemStorage {
  boxId: number;
  name: string;
  brandId: number;
  updatedAt?: string | Date;
  quantity: number;
  expires: boolean;
  expiresOn: string | Date;
}

export interface IStorage {
  boxId: number;
  itemId: number;
  brandId: number;
  quantity: number;
  expires: boolean;
  expiresOn?: string | Date;
}

export interface StorageForm {
  boxId: FormControl<number>;  
  itemId: FormControl<number>;
  brandId: FormControl<number>;  
  quantity: FormControl<number>;
  expires: FormControl<boolean>;
  expiresOn: FormControl<string | Date | null>;
}

export interface BoxStep {
  boxId: FormControl<number>;
}

export interface ItemStep {
  itemId: FormControl<number>;
  brandId: FormControl<number>;
}

export interface DetailsStep {
  quantity: FormControl<number>;
  expires: FormControl<boolean>;
  expiresOn: FormControl<string | Date | null>;
}