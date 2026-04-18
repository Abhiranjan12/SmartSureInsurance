import { Routes } from '@angular/router';
import { authGuard, adminGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./components/register/register.component').then(m => m.RegisterComponent) },

  { path: 'dashboard', loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent), canActivate: [authGuard] },
  { path: 'policies', loadComponent: () => import('./components/my-policies/my-policies.component').then(m => m.MyPoliciesComponent), canActivate: [authGuard] },
  { path: 'buy-policy', loadComponent: () => import('./components/buy-policy/buy-policy.component').then(m => m.BuyPolicyComponent), canActivate: [authGuard] },
  { path: 'claims', loadComponent: () => import('./components/my-claims/my-claims.component').then(m => m.MyClaimsComponent), canActivate: [authGuard] },
  { path: 'claims/new', loadComponent: () => import('./components/new-claim/new-claim.component').then(m => m.NewClaimComponent), canActivate: [authGuard] },

  { path: 'admin/dashboard', loadComponent: () => import('./components/admin/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent), canActivate: [adminGuard] },
  { path: 'admin/claims', loadComponent: () => import('./components/admin/admin-claims/admin-claims.component').then(m => m.AdminClaimsComponent), canActivate: [adminGuard] },
  { path: 'admin/policies', loadComponent: () => import('./components/admin/admin-policies/admin-policies.component').then(m => m.AdminPoliciesComponent), canActivate: [adminGuard] },
  { path: 'admin/users', loadComponent: () => import('./components/admin/admin-users/admin-users.component').then(m => m.AdminUsersComponent), canActivate: [adminGuard] },
  { path: 'admin/reports', loadComponent: () => import('./components/admin/admin-reports/admin-reports.component').then(m => m.AdminReportsComponent), canActivate: [adminGuard] },

  { path: '**', redirectTo: '/login' }
];
