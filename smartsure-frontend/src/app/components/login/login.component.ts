import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  email = '';
  password = '';
  otpCode = '';
  step: 'login' | 'otp' = 'login';
  loading = false;
  error = '';
  message = '';

  constructor(private auth: AuthService, private router: Router) {}

  onLogin() {
    this.loading = true;
    this.error = '';
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: res => {
        this.message = res.message;
        this.step = 'otp';
        this.loading = false;
      },
      error: err => {
        this.error = err.error?.message || err.message || 'Login failed. Check if backend is running on port 5001.';
        console.error('Login error:', err);
        this.loading = false;
      }
    });
  }

  onVerifyOtp() {
    this.loading = true;
    this.error = '';
    this.auth.verifyLoginOtp({ email: this.email, otpCode: this.otpCode }).subscribe({
      next: () => {
        this.loading = false;
        if (this.auth.isAdmin()) {
          this.router.navigate(['/admin/dashboard']);
        } else {
          this.router.navigate(['/dashboard']);
        }
      },
      error: err => {
        this.error = err.error?.message || 'Invalid OTP';
        this.loading = false;
      }
    });
  }
}
