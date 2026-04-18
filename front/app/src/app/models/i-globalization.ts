export interface ILanguage {
  languageCode: string;
  name: string;
  isDefault: boolean;
}

export interface ITranslationDictionary {
  [context: string]: {
    [key: string]: string;
  };
}