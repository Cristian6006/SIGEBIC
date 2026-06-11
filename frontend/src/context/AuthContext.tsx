import { createContext, useCallback, useEffect, useMemo, useState, type ReactNode } from 'react';
import * as authApi from '@/api/auth.api';
import { setLogoutFunction, setToken } from '@/lib/axios';
import type { LoginRequest, Usuario } from '@/types/auth.types';

export interface AuthContextType {
  usuario: Usuario | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (data: LoginRequest) => Promise<void>;
  logout: () => Promise<void>;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [usuario, setUsuario] = useState<Usuario | null>(null);
  const [token, setTokenState] = useState<string | null>(null);

  const isAuthenticated = useMemo(() => !!token, [token]);

  const login = useCallback(async (data: LoginRequest) => {
    const response = await authApi.login(data);
    const newUser: Usuario = {
      token: response.token,
      email: response.email,
      nombreCompleto: response.nombreCompleto,
      rol: response.rol,
      expiracion: response.expiracion,
    };
    setTokenState(response.token);
    setToken(response.token);
    setUsuario(newUser);
  }, []);

  const logout = useCallback(async () => {
    try {
      await authApi.logout();
    } catch {
      // Even if the server call fails, we clean local state
    } finally {
      setTokenState(null);
      setToken(null);
      setUsuario(null);
    }
  }, []);

  // Register the logout function for axios interceptor
  useEffect(() => {
    setLogoutFunction(logout);
  }, [logout]);

  const contextValue = useMemo<AuthContextType>(
    () => ({
      usuario,
      token,
      isAuthenticated,
      login,
      logout,
    }),
    [usuario, token, isAuthenticated, login, logout]
  );

  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
}