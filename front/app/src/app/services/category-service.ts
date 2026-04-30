import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal, Signal } from '@angular/core';
import { environment } from '@core';
import { Observable, tap } from 'rxjs';
import { EntityScope } from '../models/e-entity-scope';
import { ICategory } from '../models/i-category';
import { InListValidator } from '../shared/validators/in-list-validator';

@Injectable({
    providedIn: 'root'
})
export class CategoryService {
    private http = inject(HttpClient);
    private readonly apiUrl = environment.endpoint.category;

    private _categories = signal<ICategory[]>([]);
    public categories = this._categories.asReadonly();

    constructor() {
        this.refresh();
    }

    public refresh() {
        this.http.get<ICategory[]>(this.apiUrl).subscribe(data => {
            this._categories.set(data);
        });
    }

    public updateLocalCategories(newList: ICategory[]) {
        this._categories.set(newList);
    }

    reorder(categoryId: number, newOrder: number): Observable<boolean> {
        return this.http.patch<boolean>(`${this.apiUrl}/${categoryId}/reorder/${newOrder}`, {});
    }

    public itemCategories = computed(() =>
        this.categories().filter(c => (c.includedIn & EntityScope.Item) === EntityScope.Item)
    );

    public boxCategories = computed(() =>
        this.categories().filter(c => (c.includedIn & EntityScope.Box) === EntityScope.Box)
    );

    save(category: Partial<ICategory>) {
        const obs = category.categoryId
            ? this.http.put<ICategory>(`${this.apiUrl}/${category.categoryId}`, category)
            : this.http.post<ICategory>(this.apiUrl, category);
        return obs.pipe(tap(() => this.refresh()));
    }

    delete(categoryId: number) {
        return this.http.delete(`${this.apiUrl}/${categoryId}`)
            .pipe(tap(() => this.refresh()));
    }

    public hasScope(category: ICategory, scope: EntityScope): boolean {
        return (category.includedIn & scope) === scope;
    }

    public getCategoryById(categoryId: number): ICategory | undefined {
        return this.categories().find(c => c.categoryId === categoryId);
    }

    public getCategoriesByScope(scope: EntityScope): Signal<ICategory[]> {
        return computed(() => {
            const all = this.categories();
            if (scope === EntityScope.General) return all;
            if (scope === EntityScope.None) return [];
            return all.filter(c => (c.includedIn & scope) === scope);
        });
    }

    readonly validators = {
        exists: () => InListValidator.exists(this.categories, 'categoryId', 1),
        existsInScope: (scope: EntityScope = EntityScope.General) => {
            return computed(() => {
                let sourceSignal: Signal<ICategory[]>;
                switch (scope) {
                    case EntityScope.Item:
                        sourceSignal = this.itemCategories;
                        break;
                    case EntityScope.Box:
                        sourceSignal = this.boxCategories;
                        break;
                    default:
                        sourceSignal = this.categories;
                        break;
                }
                return InListValidator.exists(sourceSignal, 'categoryId', 1);
            })();
        }
    };

}