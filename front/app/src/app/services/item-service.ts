import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IItem, IItemContentIn } from '../models/i-item';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  
  private http = inject(HttpClient);
  readonly apiUrl = '/api/items/';

  getItems(): Observable<IItem[]> {      
    return this.http.get<IItem[]>(this.apiUrl) ?? [];
  }

  getItemBy(itemId: number): Observable<IItem> {
    return this.http.get<IItem>(this.apiUrl + itemId);
  }

  getEmptyItem(): Observable<IItem> {      
    return this.http.get<IItem>(`${this.apiUrl}empty`);
  }

  saveItem(item: IItem): Observable<IItem> {
    if (item.itemId > 0) {
      return this.http.put<IItem>(`${this.apiUrl}${item.itemId}`, item);
    }
    return this.http.post<IItem>(`${this.apiUrl}`, item);
  }

  deleteItem(itemId: number): Observable<void> {    
    return this.http.delete<void>(`${this.apiUrl}${itemId}`);
  }

  getTempPhotoBy(fileGuid: string): string {
    if(!fileGuid) return '/cdn/img/item/0.png';
    return `/cdn/temp/${fileGuid}.png`
  }

  assignImage(itemId: number, fileGuid: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}${itemId}/assign-image/${fileGuid}`, {});
  }

  parseItemInBox(jsonString: string | null | undefined): IItemContentIn[] {
    if (!jsonString) return [];    
    try {
      return typeof jsonString === 'string' ? JSON.parse(jsonString) : jsonString;
    } catch (error) {
      console.error("Error parsing Item.InBox:", error);
      return [];
    }
  }

}
