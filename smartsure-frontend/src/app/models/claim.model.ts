export interface Claim {
  id: string;
  customerId: string;
  policyId: string;
  claimNumber: string;
  incidentDescription: string;
  incidentDate: string;
  claimAmount: number;
  status: string;
  rejectionReason?: string;
  adminRemarks?: string;
  createdAt: string;
}

export interface CreateClaimRequest {
  policyId: string;
  incidentDescription: string;
  incidentDate: string;
  claimAmount: number;
}

export interface UpdateClaimStatusRequest {
  status: string;
  rejectionReason?: string;
  adminRemarks?: string;
}
