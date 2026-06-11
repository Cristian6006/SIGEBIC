import { useState } from 'react';
//import { useAuth } from '@/hooks/useAuth';
import { useCambiarPassword, usePerfil } from '@/hooks/useUsuarios';
import type { AxiosError } from 'axios';

interface BackendValidationErrors {
  errors?: Record<string, string[]>;
  error?: string;
}

export default function PerfilPage() {
  //const { usuario } = useAuth();
  const { data: perfil, isLoading, isError } = usePerfil();
  const cambiarPassword = useCambiarPassword();

  // Form de cambio de contraseña
  const [passwordActual, setPasswordActual] = useState('');
  const [nuevoPassword, setNuevoPassword] = useState('');
  const [confirmarPassword, setConfirmarPassword] = useState('');
  const [fieldErrors, setFieldErrors] = useState<Record<string, string>>({});
  const [successMsg, setSuccessMsg] = useState('');

  function validatePasswordForm(): boolean {
    const errors: Record<string, string> = {};

    if (!passwordActual) errors['passwordActual'] = 'La contraseña actual es requerida.';

    if (!nuevoPassword) {
      errors['nuevoPassword'] = 'La nueva contraseña es requerida.';
    } else if (nuevoPassword.length < 8) {
      errors['nuevoPassword'] = 'La contraseña debe tener al menos 8 caracteres.';
    } else if (!/(?=.*[A-Z])(?=.*[a-z])(?=.*\d)/.test(nuevoPassword)) {
      errors['nuevoPassword'] = 'Debe contener mayúscula, minúscula y número.';
    }

    if (nuevoPassword !== confirmarPassword) {
      errors['confirmarPassword'] = 'Las contraseñas no coinciden.';
    }

    setFieldErrors(errors);
    return Object.keys(errors).length === 0;
  }

  async function handleCambiarPassword(e: React.FormEvent) {
    e.preventDefault();
    setSuccessMsg('');

    if (!validatePasswordForm()) return;

    try {
      await cambiarPassword.mutateAsync({
        id: perfil!.id,
        data: {
          passwordActual,
          nuevoPassword,
        },
      });
      setPasswordActual('');
      setNuevoPassword('');
      setConfirmarPassword('');
      setSuccessMsg('Contraseña actualizada correctamente.');
    } catch (error) {
      const axiosError = error as AxiosError<BackendValidationErrors>;
      const serverErrors = axiosError.response?.data?.errors;
      const errorMsg = axiosError.response?.data?.error;

      if (serverErrors) {
        const mapped: Record<string, string> = {};
        for (const [key, messages] of Object.entries(serverErrors)) {
          mapped[key] = Array.isArray(messages) ? messages[0] : messages;
        }
        setFieldErrors(mapped);
      } else if (errorMsg) {
        setFieldErrors({ passwordActual: errorMsg });
      }
    }
  }

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <p className="text-sm text-muted-foreground">Cargando perfil...</p>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="flex items-center justify-center h-64">
        <p className="text-sm text-destructive">Ocurrió un error al cargar el perfil.</p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold text-black">Mi Perfil</h1>
        <p className="text-sm text-muted-foreground">Consultá y actualizá tus datos personales.</p>
      </div>

      {/* Datos del perfil */}
      <div className="rounded-lg border bg-card p-6">
        <h2 className="mb-4 text-lg font-medium">Información personal</h2>
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div>
            <span className="text-xs text-muted-foreground">Nombre completo</span>
            <p className="font-medium">{perfil?.nombre} {perfil?.apellido}</p>
          </div>
          <div>
            <span className="text-xs text-muted-foreground">Email</span>
            <p className="font-medium">{perfil?.email}</p>
          </div>
          <div>
            <span className="text-xs text-muted-foreground">Documento</span>
            <p className="font-medium">{perfil?.numeroDocumento || '—'}</p>
          </div>
          <div>
            <span className="text-xs text-muted-foreground">Teléfono</span>
            <p className="font-medium">{perfil?.telefono || '—'}</p>
          </div>
          <div>
            <span className="text-xs text-muted-foreground">Rol</span>
            <p className="font-medium">{perfil?.nombreRol}</p>
          </div>
          <div>
            <span className="text-xs text-muted-foreground">Fecha de registro</span>
            <p className="font-medium">
              {perfil?.fechaRegistro
                ? new Date(perfil.fechaRegistro).toLocaleDateString('es-AR')
                : '—'}
            </p>
          </div>
        </div>
      </div>

      {/* Cambiar contraseña */}
      <div className="rounded-lg border bg-card p-6">
        <h2 className="mb-4 text-lg font-medium">Cambiar contraseña</h2>

        {successMsg && (
          <div className="mb-4 rounded-md bg-green-50 p-3 text-sm text-green-700 border border-green-200">
            {successMsg}
          </div>
        )}

        <form onSubmit={handleCambiarPassword} className="space-y-4 max-w-md">
          <div>
            <label className="text-sm font-medium">Contraseña actual</label>
            <input
              type="password"
              value={passwordActual}
              onChange={(e) => setPasswordActual(e.target.value)}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['passwordActual'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['passwordActual']}</p>
            )}
          </div>

          <div>
            <label className="text-sm font-medium">Nueva contraseña</label>
            <input
              type="password"
              value={nuevoPassword}
              onChange={(e) => setNuevoPassword(e.target.value)}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['nuevoPassword'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['nuevoPassword']}</p>
            )}
          </div>

          <div>
            <label className="text-sm font-medium">Confirmar nueva contraseña</label>
            <input
              type="password"
              value={confirmarPassword}
              onChange={(e) => setConfirmarPassword(e.target.value)}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['confirmarPassword'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['confirmarPassword']}</p>
            )}
          </div>

          <button
            type="submit"
            disabled={cambiarPassword.isPending}
            className="rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90 disabled:opacity-50"
          >
            {cambiarPassword.isPending ? 'Guardando...' : 'Cambiar contraseña'}
          </button>
        </form>
      </div>
    </div>
  );
}