import { FormControl } from "@angular/forms";
import { EntityScope } from "./e-entity-scope";

export interface ICategory {
    categoryId: number;
    name: string;
    icon: string;
    color: string;
    order: number;
    includedIn: EntityScope;
}

export interface CategoryForm {
    name: FormControl<string>;
    icon: FormControl<string>;
    color: FormControl<string>;
    includedIn: FormControl<number[]>;
}

export const EMPTY_CATEGORY: ICategory = {
    categoryId: 0,
    name: '',
    icon: '',
    color: '',
    order: 0,
    includedIn: EntityScope.None
}