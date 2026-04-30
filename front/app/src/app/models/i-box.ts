import { FormControl } from "@angular/forms";

export interface IBox {
    boxId: number;
    parentBoxId: number | null;
    name: string;
    brandId: number;
    categoryId: number;
    height: number;
    width: number;
    depth: number;
    volume?: number;
    notes: string;
    createdAt?: string | Date;
    updatedAt?: string | Date;
    fullPath?: string;
    hasChildren?: boolean;
    hasItems?: boolean;
    canBeDeleted?: boolean;
}

export interface IBoxLookup {
    boxId: number;
    name: string;
    updatedAt: string | Date;
    indent: number;
}

export interface IBoxTransfer {
    boxId: number | null;
    name: string;
    updatedAt: string | Date;
    indent: number;
    isSelectable: boolean;
}

export interface BoxForm {
    parentBoxId: FormControl<number | null>;
    name: FormControl<string>;
    categoryId: FormControl<number>;
    brandId: FormControl<number>;
    notes: FormControl<string>;
    height: FormControl<number>;
    width: FormControl<number>;
    depth: FormControl<number>;
}

export interface BoxTransferForm {
    newParentId: FormControl<number | number[] | null>;
    confirmMove: FormControl<boolean>;
}

export interface IBoxFullPath {
    boxId: number;
    name: string;
}