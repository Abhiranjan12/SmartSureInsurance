import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ClaimService } from '../../services/claim.service';
import { PolicyService } from '../../services/policy.service';
import { Policy } from '../../models/policy.model';

@Component({
  selector: 'app-new-claim',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './new-claim.component.html'
})
export class NewClaimComponent implements OnInit {
  policies: Policy[] = [];
  policyId = '';
  incidentDescription = '';
  incidentDate = '';
  claimAmount = 0;
  selectedFile: File | null = null;
  createdClaimId = '';
  step = 1;
  loading = false;
  error = '';
  success = '';
  today = new Date().toISOString().split('T')[0];

  constructor(
    private claimService: ClaimService,
    private policyService: PolicyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.policyService.getMyPolicies().subscribe(p => {
      this.policies = p.filter(pol => pol.status === 'Active');
    });
    this.route.queryParams.subscribe(params => {
      if (params['policyId']) this.policyId = params['policyId'];
    });
  }

  onFileChange(event: any) {
    this.selectedFile = event.target.files[0];
  }

  createClaim() {
    this.loading = true;
    this.error = '';
    this.claimService.createClaim({
      policyId: this.policyId,
      incidentDescription: this.incidentDescription,
      incidentDate: new Date(this.incidentDate).toISOString(),
      claimAmount: this.claimAmount
    }).subscribe({
      next: claim => {
        this.createdClaimId = claim.id;
        this.step = 2;
        this.loading = false;
      },
      error: err => { this.error = err.error?.message || 'Failed to create claim'; this.loading = false; }
    });
  }

  uploadAndSubmit() {
    this.loading = true;
    if (this.selectedFile) {
      this.claimService.uploadDocument(this.createdClaimId, this.selectedFile).subscribe({
        next: () => this.submitClaim(),
        error: () => this.submitClaim()
      });
    } else {
      this.submitClaim();
    }
  }

  submitClaim() {
    this.claimService.submitClaim(this.createdClaimId).subscribe({
      next: () => {
        this.success = 'Claim submitted successfully!';
        this.loading = false;
        setTimeout(() => this.router.navigate(['/claims']), 2000);
      },
      error: err => { this.error = err.error?.message || 'Submit failed'; this.loading = false; }
    });
  }
}
