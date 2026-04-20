import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { ILanguage, ITranslationDictionary } from '@models';
import { firstValueFrom, Observable, switchMap, tap } from 'rxjs';
import { GlobalizationKey } from '../core/types/globalization-keys';

@Injectable({
  providedIn: 'root'
})
export class GlobalizationService {
  private http: HttpClient = inject(HttpClient);
  private readonly apiUrl = 'api/globalization/'; 

  private _translations = signal<ITranslationDictionary>({});
  private _currentLanguage = signal<string>(localStorage.getItem('language') || 'en');
  private _languages = signal<ILanguage[]>([]);

  public translations = this._translations.asReadonly();
  public currentLanguage = this._currentLanguage.asReadonly();
  public languages = this._languages.asReadonly();

  public getLanguageLabelKey(languageCode: string): GlobalizationKey {
    const formattedCode = languageCode.toUpperCase().replace('-', '_');
    return `Menu.LANG_${formattedCode}` as GlobalizationKey;
  }

  public currentLanguageLabelKey = computed(() => {
    return this.getLanguageLabelKey(this.currentLanguage());
  });

  updateLanguage(languageCode: string) {
    localStorage.setItem('language', languageCode);
    this._currentLanguage.set(languageCode);
    this.loadLocales().subscribe();
  }

  getLanguages(): Observable<ILanguage[]> {
    return this.http.get<ILanguage[]>(`${this.apiUrl}languages`);
  }

  translate(fullKey: GlobalizationKey | string, params?: any[]): string;
  translate(context: string, key: string, params?: any[]): string;  
  translate(arg1: GlobalizationKey | string, arg2OrParams?: string | any[], params: any[] = []): string {
    let context: string;
    let key: string;
    let actualParams: any[];

    if (Array.isArray(arg2OrParams) || arg2OrParams === undefined) {
      // translate('Context.Key', [params])
      if(!arg1.includes('.')) return arg1;
      [context, key] = arg1.split('.');
      actualParams = arg2OrParams || [];
      if(key.length == 0) return arg1;
    } else {
      // translate('Context', 'Key', [params])
      context = arg1;
      key = arg2OrParams;
      actualParams = params;
    }

    const contextData = this.translations()[context];
    const translation = contextData ? contextData[key] : null;

    if (!translation) return `[${context}.${key}]`; 
    if (actualParams.length === 0) return translation;

    return translation.replace(/{(\d+)}/g, (match, index) => {
      const val = actualParams[index];
      return typeof val !== 'undefined' ? val : match;
    });
  }

  refreshServerCache(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}refresh`, {});
  }
  
  public async initializeApp(): Promise<void> {
    try {
      await firstValueFrom(
        this.getLanguages().pipe(
          tap(langs => {
            this._languages.set(langs);
            const storedLang = localStorage.getItem('language');
            if (!storedLang) {
              const defaultLang = langs.find(l => l.isDefault)?.languageCode || langs[0]?.languageCode || 'en';
              localStorage.setItem('language', defaultLang);
              this._currentLanguage.set(defaultLang);
            } else {
              this._currentLanguage.set(storedLang);
            }
          }),
          switchMap(() => this.loadLocales())
        )
      );
      return void 0;
    } catch {
      return void 0;
    } 
  }  

  private loadLocales(): Observable<ITranslationDictionary> {
    return this.http.get<ITranslationDictionary>(`${this.apiUrl}locales`).pipe(
      tap(data => this._translations.set(data))
    );
  }

}