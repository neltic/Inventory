import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '@core';
import { Entity, EntityName, IAudit } from '@models';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class AuditService {
    private http = inject(HttpClient);
    private readonly apiUrl = environment.endpoint.audit;

    getHistory(entity: Entity, recordId: string): Observable<IAudit[]> {
        const entityName = EntityName[entity];
        return this.http.get<IAudit[]>(`${this.apiUrl}/${entityName}/${recordId}`);
    }
}