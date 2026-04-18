import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClaimService } from '../../../services/claim.service';
import { Claim } from '../../../models/claim.model';

@Component({
  selector: 'app-admin-claims',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-claims.component.html'
})
export class AdminClaimsComponent implements OnInit {
  claims: Claim[] = [];
  loading = true;
  selectedClaim: Claim | null = null;
  newStatus = '';
  rejectionReason = '';
  adminRemarks = '';
  updating = false;
  error = '';

  constructor(private claimService: ClaimService) {}

  ngOnInit() {
    this.claimService.getAllClaims().subscribe({
      next: c => { this.claims = c; this.loading = false; },
      error: () => this.loading = false
    });
  }

  openModal(claim: Claim) {
    this.selectedClaim = claim;
    this.newStatus = '';
    this.rejectionReason = '';
    this.adminRemarks = '';
    this.error = '';
  }

  closeModal() { this.selectedClaim = null; }

  updateStatus() {
    if (!this.selectedClaim || !this.newStatus) return;
    this.updating = true;
    this.claimService.updateClaimStatus(this.selectedClaim.id, {
      status: this.newStatus,
      rejectionReason: this.rejectionReason,
      adminRemarks: this.adminRemarks
    }).subscribe({
      next: updated => {
        const idx = this.claims.findIndex(c => c.id === updated.id);
        if (idx !== -1) this.claims[idx] = updated;
        this.updating = false;
        this.closeModal();
      },
      error: err => { this.error = err.error?.message || 'Update failed'; this.updating = false; }
    });
  }
}
