import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PolicyService } from '../../../services/policy.service';
import { Policy } from '../../../models/policy.model';

@Component({
  selector: 'app-admin-policies',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-policies.component.html'
})
export class AdminPoliciesComponent implements OnInit {
  policies: Policy[] = [];
  loading = true;
  updating = false;
  error = '';

  constructor(private policyService: PolicyService) {}

  ngOnInit() {
    this.policyService.getAllPolicies().subscribe({
      next: p => { this.policies = p; this.loading = false; },
      error: () => this.loading = false
    });
  }

  cancelPolicy(id: string) {
    if (!confirm('Cancel this policy?')) return;
    this.policyService.updatePolicyStatus(id, 'Cancelled').subscribe({
      next: updated => {
        const idx = this.policies.findIndex(p => p.id === id);
        if (idx !== -1) this.policies[idx] = updated;
      },
      error: err => this.error = err.error?.message || 'Failed'
    });
  }
}
