import { GlobalizationKey } from "../core/types/globalization-keys";

export enum EntityScope {
  None = 0,
  Item = 1,
  Box = 2,
  General = 255
}

export interface ScopeOption {
  value: EntityScope;
  label: GlobalizationKey;
  icon: string;
}

export const SCOPE_NONE_OPTION: ScopeOption = {
  value: EntityScope.None, label: 'Scope.NONE', icon: 'layers_clear'
}

export const SCOPE_OPTIONS: ScopeOption[] = [
  { value: EntityScope.General, label: 'Scope.ALL', icon: 'all_inclusive' },
  { value: EntityScope.Item, label: 'Scope.ITEM', icon: 'art_track' },
  { value: EntityScope.Box, label: 'Scope.BOX', icon: 'inventory' }
];

export const SCOPE_SELECTABLE_OPTIONS: ScopeOption[] = [
  { value: EntityScope.Item, label: 'Scope.ITEM', icon: 'art_track' },
  { value: EntityScope.Box, label: 'Scope.BOX', icon: 'inventory' }
];