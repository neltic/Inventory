export enum EntityScope {
  None = 0,
  Item = 1,
  Box = 2,
  General = 255
}

export interface ScopeOption {
  value: EntityScope;
  label: string;
  icon: string;
}

export const SCOPE_NONE_OPTION: ScopeOption = {
  value: EntityScope.None, label: 'None', icon: 'layers_clear'
}

export const SCOPE_OPTIONS: ScopeOption[] = [
  { value: EntityScope.General, label: 'All', icon: 'all_inclusive' },
  { value: EntityScope.Item, label: 'Item', icon: 'art_track' },
  { value: EntityScope.Box, label: 'Box', icon: 'inventory' }
];

export const SCOPE_SELECTABLE_OPTIONS: ScopeOption[] = [
  { value: EntityScope.Item, label: 'Item', icon: 'art_track' },
  { value: EntityScope.Box, label: 'Box', icon: 'inventory' }
];