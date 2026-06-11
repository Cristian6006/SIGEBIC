export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  nombreCompleto: string;
  rol: string;
  expiracion: string;
}

export interface Usuario {
  token: string;
  email: string;
  nombreCompleto: string;
  rol: string;
  expiracion: string;
}