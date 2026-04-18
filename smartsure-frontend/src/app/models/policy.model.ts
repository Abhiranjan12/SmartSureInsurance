export interface PolicyType {
  id: string;
  name: string;
  description: string;
  basePremium: number;
  coverageYears: number;
  isActive: boolean;
}

export interface Policy {
  id: string;
  customerId: string;
  policyNumber: string;
  policyTypeName: string;
  status: string;
  startDate: string;
  endDate: string;
  premiumAmount: number;
  coverageAmount: number;
  createdAt: string;
}

export interface BuyPolicyRequest {
  policyTypeId: string;
  coverageAmount: number;
  durationYears: number;
  customerAge: number;
}

export interface PremiumResult {
  policyTypeId: string;
  policyTypeName: string;
  coverageAmount: number;
  durationYears: number;
  calculatedPremium: number;
  monthlyPremium: number;
}
