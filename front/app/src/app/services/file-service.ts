import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FileService {

  private http = inject(HttpClient);  
  private readonly apiUrl = environment.endpoint.upload;

  uploadTempImage(file: File): Observable<{ fileGuid: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ fileGuid: string }>(`${this.apiUrl}/image`, formData);
  }
}