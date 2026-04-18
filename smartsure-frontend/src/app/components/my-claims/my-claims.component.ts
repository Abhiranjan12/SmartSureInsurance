import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ClaimService } from '../../services/claim.service';
import { Claim } from '../../models/claim.model';

@Component({
  selector: 'app-my-claims',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './my-claims.component.html'
})
export class MyClaimsComponent implements OnInit {
  claims: Claim[] = [];
  loading = true;

  constructor(private claimService: ClaimService) {}

  ngOnInit() {
    this.claimService.getMyClaims().subscribe({
      next: c => { this.claims = c; this.loading = false; },
      error: () => this.loading = false
    });
  }

  submitClaim(id: string) {
    this.claimService.submitClaim(id).subscribe({
      next: updated => {
        const idx = this.claims.findIndex(c => c.id === id);
        if (idx !== -1) this.claims[idx] = updated;
      }
    });
  }
}
