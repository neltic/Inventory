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
    map(() => this.extractAllParams()), 
    startWith(this.extractAllParams())
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

  private extractAllParams(): RouteParams {
    let root = this.router.routerState.snapshot.root;
    let params: any = {};
    let queryParams: any = root.queryParams;     
    let stack = [root];
    while (stack.length > 0) {
      const node = stack.pop()!;
      params = { ...params, ...node.params };
      if (node.firstChild) stack.push(node.firstChild);
    }
    return { ...params, action: queryParams['action'] };
  }

  public clearQueryAction() {
    this.router.navigate([], { 
        queryParams: { action: null }, 
        queryParamsHandling: 'merge' 
    });
  }
   
}