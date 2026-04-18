import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Claim, CreateClaimRequest, UpdateClaimStatusRequest } from '../models/claim.model';

@Injectable({ providedIn: 'root' })
export class ClaimService {
  private api = environment.claimsApiUrl;
  private adminApi = environment.adminApiUrl;

  constructor(private http: HttpClient) {}

  getMyClaims(): Observable<Claim[]> {
    return this.http.get<Claim[]>(`${this.api}/claims/my`);
  }

  getAllClaims(): Observable<Claim[]> {
    return this.http.get<Claim[]>(`${this.api}/claims`);
  }

  getClaimById(id: string): Observable<Claim> {
    return this.http.get<Claim>(`${this.api}/claims/${id}`);
  }

  createClaim(data: CreateClaimRequest): Observable<Claim> {
    return this.http.post<Claim>(`${this.api}/claims`, data);
  }

  submitClaim(id: string): Observable<Claim> {
    return this.http.patch<Claim>(`${this.api}/claims/${id}/submit`, {});
  }

  updateClaimStatus(id: string, data: UpdateClaimStatusRequest): Observable<Claim> {
    return this.http.patch<Claim>(`${this.adminApi}/claims/${id}/status`, data);
  }

  uploadDocument(claimId: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.api}/claims/${claimId}/documents/upload`, formData);
  }

  getDocuments(claimId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/claims/${claimId}/documents`);
  }
}
