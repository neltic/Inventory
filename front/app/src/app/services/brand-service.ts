import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal, Signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { EntityScope } from '../models/e-entity-scope';
import { IBrand } from '../models/i-brand';
import { InListValidator } from '../shared/validators/in-list-validator';

@Injectable({
  providedIn: 'root'
})
export class BrandService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'api/brands/'; 

  private _brands = signal<IBrand[]>([]);
  public brands = this._brands.asReadonly();

  constructor() {
    this.refresh();
  }

  public refresh() {    
    this.http.get<IBrand[]>(this.apiUrl).subscribe(data => {
      this._brands.set(data);
    });
  }

  public itemBrands = computed(() => 
    this.brands().filter(b => (b.includedIn & EntityScope.Item) === EntityScope.Item)
  );

  public boxBrands = computed(() => 
    this.brands().filter(b => (b.includedIn & EntityScope.Box) === EntityScope.Box)
  );

  save(brand: Partial<IBrand>): Observable<IBrand> {
    const obs = (brand.brandId ?? -1) > -1
      ? this.http.put<IBrand>(`${this.apiUrl}${brand.brandId}`, brand)
      : this.http.post<IBrand>(this.apiUrl, brand);    
    return obs.pipe(tap(() => this.refresh()));
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}${id}`)
      .pipe(tap(() => this.refresh()));
  }

  hasScope(brand: IBrand, scope: EntityScope): boolean {
    return (brand.includedIn & scope) === scope;
  }

  getBrandById(brandId: number): IBrand | undefined {
    return this.brands().find(b => b.brandId === brandId);
  }

  public getBrandsByScope(scope: EntityScope): Signal<IBrand[]> {
    return computed(() => {
      const all = this.brands();      
      if (scope === EntityScope.General) return all; 
      if (scope === EntityScope.None) return [];
      return all.filter(c => (c.includedIn & scope) === scope);
    });
  }

  readonly validators = {
    exists: () => InListValidator.exists(this.brands, 'brandId', 0), 
    existsInScope: (scope: EntityScope = EntityScope.General) => {
      return computed(() => {        
        let sourceSignal: Signal<IBrand[]>;        
        switch (scope) {
          case EntityScope.Item:
            sourceSignal = this.itemBrands;
            break;
          case EntityScope.Box:
            sourceSignal = this.boxBrands;
            break;
          default:
            sourceSignal = this.brands;
            break;
        }        
        return InListValidator.exists(sourceSignal, 'brandId', 0);
      })();
    }
  };

}