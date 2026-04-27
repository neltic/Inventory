import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '@core';
import { Observable } from 'rxjs';
import { IItemInBox, IItemStorage, IStorage } from '../models/i-storage';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  
  private http = inject(HttpClient);
  private readonly apiUrl = environment.endpoint.storage;
  
  itemsInBox = signal<IItemInBox[]>([]);
  itemStorage = signal<IItemStorage[]>([]);

  getItemsByBox(boxId: number): Observable<IItemInBox[]> {
    return this.http.get<IItemInBox[]>(`${this.apiUrl}/boxes/${boxId}`);
  }

  remove(boxId: number, itemId: number, brandId: number): Observable<boolean> {    
    return this.http.delete<boolean>(`${this.apiUrl}/boxes/${boxId}/items/${itemId}/brands/${brandId}`);
  }

  getStorageByItem(itemId: number): Observable<IItemStorage[]> {
    return this.http.get<IItemStorage[]>(`${this.apiUrl}/items/${itemId}`);
  }

  getStorage(boxId: number, itemId: number, brandId: number): Observable<IStorage> {
    return this.http.get<IStorage>(`${this.apiUrl}/boxes/${boxId}/items/${itemId}/brands/${brandId}`);
  }

  saveStorage(storage: IStorage): Observable<any> {    
    return this.http.put<any>(`${this.apiUrl}`, storage);    
  }

}
