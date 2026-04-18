import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PolicyService } from '../../services/policy.service';
import { Policy } from '../../models/policy.model';

@Component({
  selector: 'app-my-policies',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './my-policies.component.html'
})
export class MyPoliciesComponent implements OnInit {
  policies: Policy[] = [];
  loading = true;

  constructor(private policyService: PolicyService) {}

  ngOnInit() {
    this.policyService.getMyPolicies().subscribe({
      next: p => { this.policies = p; this.loading = false; },
      error: () => this.loading = false
    });
  }
}
