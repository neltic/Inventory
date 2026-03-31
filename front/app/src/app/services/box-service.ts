import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IBox, IBoxFullPath, IBoxLookup } from '../models/i-box';

@Injectable({
  providedIn: 'root',
})
export class BoxService {
  
  private http = inject(HttpClient);
  readonly apiUrl = '/api/boxes/';

  getBoxesBy(parentBoxId: number | null): Observable<IBox[]> {
    let params = new HttpParams();  
    if (parentBoxId !== undefined && parentBoxId !== null) {
      params = params.set('parentBoxId', parentBoxId.toString());
    }
    return this.http.get<IBox[]>(this.apiUrl, { params }) ?? [];
  }

  getBoxesLookup(): Observable<IBoxLookup[]> {    
    return this.http.get<IBoxLookup[]>(`${this.apiUrl}lookup`) ?? [];
  }

  getBoxBy(boxId: number): Observable<IBox> {
    return this.http.get<IBox>(this.apiUrl + boxId);
  }

  getEmptyBoxBy(parentBoxId: number | null): Observable<IBox> {
    let params = new HttpParams();
    if (parentBoxId !== null && parentBoxId !== undefined) {
      params = params.set('parentBoxId', parentBoxId.toString());
    }
    return this.http.get<IBox>(`${this.apiUrl}empty`, { params });
  }

  saveBox(box: IBox): Observable<IBox> {
    if (box.boxId > 0) {
      return this.http.put<IBox>(`${this.apiUrl}${box.boxId}`, box);
    }
    return this.http.post<IBox>(`${this.apiUrl}`, box);
  }

  deleteBox(boxId: number): Observable<void> {    
    return this.http.delete<void>(`${this.apiUrl}${boxId}`);
  }

  getTempPhotoBy(fileGuid: string): string {
    if(!fileGuid) return '/cdn/img/box/0.png';
    return `/cdn/temp/${fileGuid}.png`
  }

  getVolume(box: IBox | undefined): number {
    if(box) 
      return Math.round(box.width * box.height * box.depth * 100) / 100;
    return 0;
  }

  assignImage(boxId: number, fileGuid: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}${boxId}/assign-image/${fileGuid}`, {});
  }

  parseBoxFullPath(jsonString: string | null | undefined): IBoxFullPath[] {
    if (!jsonString) return [];    
    try {
      return typeof jsonString === 'string' ? JSON.parse(jsonString) : jsonString;
    } catch (error) {
      console.error("Error parsing Box.FullPath:", error);
      return [];
    }
  }

}
