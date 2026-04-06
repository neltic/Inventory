import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { IItemInBox, IItemStorage, IStorage } from '../models/i-storage';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  
  private http = inject(HttpClient);
  readonly apiStorageUrl = '/api/storages/';
  
  itemsInBox = signal<IItemInBox[]>([]);
  itemStorage = signal<IItemStorage[]>([]);

  getItemsByBox(boxId: number): Observable<IItemInBox[]> {
    return this.http.get<IItemInBox[]>(`${this.apiStorageUrl}boxes/${boxId}`);
  }

  unbindBox(boxId: number, itemId: number, brandId: number): Observable<string> {    
    return this.http.delete<string>(`${this.apiStorageUrl}boxes/${boxId}/items/${itemId}/brands/${brandId}`);
  }

  getStorageByItem(itemId: number): Observable<IItemStorage[]> {
    return this.http.get<IItemStorage[]>(`${this.apiStorageUrl}items/${itemId}`);
  }

  getStorage(boxId: number, itemId: number, brandId: number): Observable<IStorage> {
    return this.http.get<IStorage>(`${this.apiStorageUrl}boxes/${boxId}/items/${itemId}/brands/${brandId}`);
  }

  saveStorage(storage: IStorage): Observable<any> {    
    return this.http.put<any>(`${this.apiStorageUrl}`, storage);    
  }

}
