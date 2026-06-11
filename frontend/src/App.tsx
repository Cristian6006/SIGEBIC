import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from '@/context/AuthContext';
import ProtectedRoute from '@/components/shared/ProtectedRoute';
import DashboardLayout from '@/components/layout/dashboard-layout';
import LoginPage from '@/pages/auth/LoginPage';
import DashboardPage from '@/pages/DashboardPage';
import CatalogoPage from '@/pages/libros/CatalogoPage';
import GestionUsuariosPage from '@/pages/usuarios/GestionUsuariosPage';
import PerfilPage from '@/pages/usuarios/PerfilPage';

const queryClient = new QueryClient();

function UnauthorizedPage() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-background">
      <div className="text-center">
        <h1 className="text-2xl font-semibold">Acceso No Autorizado</h1>
        <p className="mt-2 text-sm text-muted-foreground">
          No tienes permisos para acceder a esta página.
        </p>
      </div>
    </div>
  );
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <AuthProvider>
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route
              path="/dashboard"
              element={
                <ProtectedRoute>
                  <DashboardLayout>
                    <DashboardPage />
                  </DashboardLayout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/catalogo"
              element={
                <ProtectedRoute>
                  <DashboardLayout>
                    <CatalogoPage />
                  </DashboardLayout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/usuarios"
              element={
                <ProtectedRoute allowedRoles={['Administrador']}>
                  <DashboardLayout>
                    <GestionUsuariosPage />
                  </DashboardLayout>
                </ProtectedRoute>
              }
            />
            <Route
              path="/perfil"
              element={
                <ProtectedRoute>
                  <DashboardLayout>
                    <PerfilPage />
                  </DashboardLayout>
                </ProtectedRoute>
              }
            />
            <Route path="/unauthorized" element={<UnauthorizedPage />} />
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
            <Route path="*" element={<Navigate to="/dashboard" replace />} />
          </Routes>
        </AuthProvider>
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;