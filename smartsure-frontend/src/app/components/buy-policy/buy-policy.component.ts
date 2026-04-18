import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PolicyService } from '../../services/policy.service';
import { AuthService } from '../../services/auth.service';
import { PolicyType, PremiumResult } from '../../models/policy.model';

@Component({
  selector: 'app-buy-policy',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './buy-policy.component.html'
})
export class BuyPolicyComponent implements OnInit {
  policyTypes: PolicyType[] = [];
  step = 1;
  selectedTypeId = '';
  coverageAmount = 500000;
  durationYears = 1;
  customerAge = 25;
  premiumResult: PremiumResult | null = null;
  loading = false;
  error = '';
  success = '';

  constructor(
    private policyService: PolicyService,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.policyService.getPolicyTypes().subscribe(types => {
      this.policyTypes = types.filter(t => t.isActive);
    });
  }

  calculatePremium() {
    if (!this.selectedTypeId) { this.error = 'Please select a policy type'; return; }
    this.loading = true;
    this.error = '';
    this.policyService.calculatePremium({
      policyTypeId: this.selectedTypeId,
      customerAge: this.customerAge,
      coverageAmount: this.coverageAmount,
      durationYears: this.durationYears
    }).subscribe({
      next: res => { this.premiumResult = res; this.step = 2; this.loading = false; },
      error: err => { this.error = err.error?.message || 'Failed to calculate'; this.loading = false; }
    });
  }

  buyPolicy() {
    this.loading = true;
    this.error = '';
    this.policyService.buyPolicy({
      policyTypeId: this.selectedTypeId,
      coverageAmount: this.coverageAmount,
      durationYears: this.durationYears,
      customerAge: this.customerAge
    }).subscribe({
      next: () => {
        this.success = 'Policy purchased successfully!';
        this.loading = false;
        setTimeout(() => this.router.navigate(['/policies']), 2000);
      },
      error: err => { this.error = err.error?.message || 'Purchase failed'; this.loading = false; }
    });
  }

  getSelectedType(): PolicyType | undefined {
    return this.policyTypes.find(t => t.id === this.selectedTypeId);
  }
}
