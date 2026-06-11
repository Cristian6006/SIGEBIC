import axios, { AxiosError } from 'axios';
import type { InternalAxiosRequestConfig } from 'axios';

// We use a reference to the store that will be set by AuthProvider
// This avoids circular dependencies
let logoutFn: (() => void) | null = null;

export function setLogoutFunction(fn: () => void) {
  logoutFn = fn;
}

export function getToken(): string | null {
  // This will be populated by the AuthContext interceptor
  return (window as unknown as { __auth_token?: string }).__auth_token ?? null;
}

export function setToken(token: string | null) {
  (window as unknown as { __auth_token?: string }).__auth_token = token ?? undefined;
}

const api = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor: attach JWT token to every request
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor: handle 401 errors
api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401 && logoutFn) {
      logoutFn();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default api;