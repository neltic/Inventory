import { Injectable, computed, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map, startWith } from 'rxjs';

interface RouteParams {
  parentBoxId?: string;
  boxId?: string;
  itemId?: string;
  // queryParams
  action?: string;
}

@Injectable({ providedIn: 'root' })
export class RouteParamsService {
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  private params$ = this.router.events.pipe(
    filter(event => event instanceof NavigationEnd),
    map(() => this.getParamsFromRoute(this.route)),
    startWith(this.getParamsFromRoute(this.route))
  );

  private params = toSignal<RouteParams>(this.params$);

  readonly parentBoxId = computed(() => 
    this.parseParam(this.params()?.parentBoxId, null)
  );

  readonly boxId = computed(() => 
    this.parseParam(this.params()?.boxId, 0)
  );

  readonly itemId = computed(() => 
    this.parseParam(this.params()?.itemId, 0)
  );

  readonly action = computed(() => this.params()?.action || null);
  
  private parseParam<T>(value: string | undefined, defaultValue: T): number | T {
    if (!value) return defaultValue;
    const parsed = parseInt(value, 10);
    return isNaN(parsed) ? defaultValue : (parsed as any);
  }

  private getParamsFromRoute(route: ActivatedRoute): RouteParams {
    let params: RouteParams = {};
    let queryParams: any = {};
    let currentRoute: ActivatedRoute | null = route;

    while (currentRoute) {      
      params = { ...params, ...currentRoute.snapshot.params };
      queryParams = { ...queryParams, ...currentRoute.snapshot.queryParams };
      currentRoute = currentRoute.firstChild;
    }
    return { ...params, ...queryParams };
  }

  public clearQueryAction() {
    this.router.navigate([], { 
        queryParams: { action: null }, 
        queryParamsHandling: 'merge' 
    });
  }
   
}