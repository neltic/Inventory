import { FormControl } from "@angular/forms";
import { EntityScope } from "./e-entity-scope";

export interface IBrand {
    brandId: number;
    name: string;
    color: string;
    background: string;
    includedIn: EntityScope;
}

export interface BrandForm {
    name: FormControl<string>;
    color: FormControl<string>;
    background: FormControl<string>;
    includedIn: FormControl<number[]>;
}

export const EMPTY_BRAND: IBrand = {
    brandId: -1,
    name: '',
    color: '',
    background: '',
    includedIn: EntityScope.None
}