import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { IBox, IBoxFullPath, IBoxLookup, IBoxTransfer } from '../models/i-box';

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

  getAvailableParentBoxesBy(targetBoxId: number | null): Observable<IBoxTransfer[]> {    
    const url = `${this.apiUrl}available-parents/${targetBoxId ?? ''}`;
    return this.http.get<IBoxTransfer[]>(url);
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

  getBoxFullPath(id: number): Observable<IBoxFullPath[]> {
      return this.http.get<IBoxFullPath[]>(`${this.apiUrl}${id}/path`);
  }

  saveBox(box: IBox): Observable<any> {
    if (box.boxId > 0) {
      return this.http.put<any>(`${this.apiUrl}${box.boxId}`, box);
    }
    return this.http.post<any>(`${this.apiUrl}`, box);
  }

  deleteBox(boxId: number): Observable<void> {    
    return this.http.delete<void>(`${this.apiUrl}${boxId}`);
  }

  moveBox(boxId: number, newParentId: number | null): Observable<void> {
    const url = `${this.apiUrl}${boxId}/move-to/${newParentId ?? ''}`;
    return this.http.patch<void>(url, {});
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

  parseBoxFullPath(jsonString: IBoxFullPath[] | string | null | undefined): IBoxFullPath[] {
    if (!jsonString) return [];    
    if (Array.isArray(jsonString)) return jsonString;
    if (typeof jsonString !== 'string') return jsonString as IBoxFullPath[];    
    try {
      return JSON.parse(jsonString);
    } catch (error) {
      console.error("Error parsing Box.FullPath:", error);
      return [];
    }
  }

  readonly validators = {  
    isValidDestination: () => {
      return (control: AbstractControl): ValidationErrors | null => {
        const value = Array.isArray(control.value) ? control.value[0] : control.value;        
        const isValid = value === null || (typeof value === 'number' && value > 0);
        return isValid ? null : { invalidDestination: true };
      };
    }
  };

}
