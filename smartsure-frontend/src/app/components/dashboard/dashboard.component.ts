import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PolicyService } from '../../services/policy.service';
import { ClaimService } from '../../services/claim.service';
import { AuthService } from '../../services/auth.service';
import { Policy } from '../../models/policy.model';
import { Claim } from '../../models/claim.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  policies: Policy[] = [];
  claims: Claim[] = [];
  loading = true;

  constructor(
    private policyService: PolicyService,
    private claimService: ClaimService,
    public auth: AuthService
  ) {}

  ngOnInit() {
    this.policyService.getMyPolicies().subscribe(p => this.policies = p);
    this.claimService.getMyClaims().subscribe(c => {
      this.claims = c;
      this.loading = false;
    });
  }

  get activePolicies() { return this.policies.filter(p => p.status === 'Active').length; }
  get pendingClaims() { return this.claims.filter(c => c.status === 'Submitted' || c.status === 'UnderReview').length; }
  get approvedClaims() { return this.claims.filter(c => c.status === 'Approved').length; }
}
