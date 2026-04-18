export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  age: number;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface VerifyOtpRequest {
  email: string;
  otpCode: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  role: string;
  userId: string;
  fullName: string;
}
