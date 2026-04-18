import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { BuyPolicyRequest, Policy, PolicyType, PremiumResult } from '../models/policy.model';

@Injectable({ providedIn: 'root' })
export class PolicyService {
  private api = environment.policyApiUrl;

  constructor(private http: HttpClient) {}

  getPolicyTypes(): Observable<PolicyType[]> {
    return this.http.get<PolicyType[]>(`${this.api}/policy-types`);
  }

  getPolicyTypeById(id: string): Observable<PolicyType> {
    return this.http.get<PolicyType>(`${this.api}/policy-types/${id}`);
  }

  calculatePremium(data: any): Observable<PremiumResult> {
    return this.http.post<PremiumResult>(`${this.api}/premiums/calculate`, data);
  }

  buyPolicy(data: BuyPolicyRequest): Observable<Policy> {
    return this.http.post<Policy>(`${this.api}/policies/buy`, data);
  }

  getMyPolicies(): Observable<Policy[]> {
    return this.http.get<Policy[]>(`${this.api}/policies/my`);
  }

  getAllPolicies(): Observable<Policy[]> {
    return this.http.get<Policy[]>(`${this.api}/policies`);
  }

  getPolicyById(id: string): Observable<Policy> {
    return this.http.get<Policy>(`${this.api}/policies/${id}`);
  }

  updatePolicyStatus(id: string, status: string): Observable<Policy> {
    return this.http.patch<Policy>(`${this.api}/policies/${id}/status`, { status });
  }

  createPolicyType(data: any): Observable<PolicyType> {
    return this.http.post<PolicyType>(`${this.api}/policy-types`, data);
  }

  updatePolicyType(id: string, data: any): Observable<PolicyType> {
    return this.http.put<PolicyType>(`${this.api}/policy-types/${id}`, data);
  }

  deletePolicyType(id: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/policy-types/${id}`);
  }
}
