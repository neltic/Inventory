import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FileService {

  private http = inject(HttpClient);
  private apiUrl = '/api/uploads';

  uploadTempImage(file: File): Observable<{ fileGuid: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ fileGuid: string }>(`${this.apiUrl}/image`, formData);
  }
}