import api from '@/lib/axios';
import type { LoginRequest, LoginResponse } from '@/types/auth.types';

export async function login(data: LoginRequest): Promise<LoginResponse> {
  const response = await api.post<LoginResponse>('/auth/login', data);
  return response.data;
}

export async function logout(): Promise<void> {
  await api.post('/auth/logout');
}