import { useState, useEffect } from 'react';
import { X } from 'lucide-react';
import { useCreateUsuario, useRoles, useUpdateUsuario } from '@/hooks/useUsuarios';
import type { UsuarioDto, CreateUsuarioRequest, UpdateUsuarioRequest } from '@/types/usuario.types';
import type { AxiosError } from 'axios';

interface UsuarioFormModalProps {
  open: boolean;
  onClose: () => void;
  usuario?: UsuarioDto | null;
}

interface BackendValidationErrors {
  errors?: Record<string, string[]>;
}

export default function UsuarioFormModal({ open, onClose, usuario }: UsuarioFormModalProps) {
  const [nombre, setNombre] = useState('');
  const [apellido, setApellido] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [telefono, setTelefono] = useState('');
  const [numeroDocumento, setNumeroDocumento] = useState('');
  const [rolId, setRolId] = useState('');

  const [fieldErrors, setFieldErrors] = useState<Record<string, string>>({});

  const createUsuario = useCreateUsuario();
  const updateUsuario = useUpdateUsuario();
  const { data: roles } = useRoles();

  const isEditing = !!usuario;

  useEffect(() => {
    if (usuario) {
      setNombre(usuario.nombre);
      setApellido(usuario.apellido);
      setEmail(usuario.email);
      setPassword('');
      setTelefono(usuario.telefono || '');
      setNumeroDocumento(usuario.numeroDocumento);
      setRolId(usuario.rolId);
    } else {
      setNombre('');
      setApellido('');
      setEmail('');
      setPassword('');
      setTelefono('');
      setNumeroDocumento('');
      setRolId('');
    }
    setFieldErrors({});
  }, [usuario, open]);

  function handleValidationErrors(error: AxiosError<BackendValidationErrors>) {
    const serverErrors = error.response?.data?.errors;
    if (serverErrors) {
      const mapped: Record<string, string> = {};
      for (const [key, messages] of Object.entries(serverErrors)) {
        mapped[key] = Array.isArray(messages) ? messages[0] : messages;
      }
      setFieldErrors(mapped);
    }
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setFieldErrors({});

    try {
      if (isEditing) {
        const datos: UpdateUsuarioRequest = {
          nombre: nombre.trim() || undefined,
          apellido: apellido.trim() || undefined,
          telefono: telefono.trim() || undefined,
          numeroDocumento: numeroDocumento.trim() || undefined,
        };
        await updateUsuario.mutateAsync({ id: usuario.id, data: datos });
      } else {
        const datos: CreateUsuarioRequest = {
          nombre: nombre.trim(),
          apellido: apellido.trim(),
          email: email.trim(),
          password: password,
          telefono: telefono.trim() || undefined,
          numeroDocumento: numeroDocumento.trim(),
          rolId: rolId,
        };
        await createUsuario.mutateAsync(datos);
      }
      onClose();
    } catch (error) {
      handleValidationErrors(error as AxiosError<BackendValidationErrors>);
    }
  }

  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
      <div className="w-full max-w-lg rounded-lg bg-card p-6 shadow-lg">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-semibold">
            {isEditing ? 'Editar usuario' : 'Nuevo usuario'}
          </h2>
          <button
            onClick={onClose}
            className="rounded-md p-1 hover:bg-muted"
            type="button"
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="text-sm font-medium">Nombre</label>
            <input
              type="text"
              value={nombre}
              onChange={(e) => setNombre(e.target.value)}
              maxLength={100}
              required
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['Nombre'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['Nombre']}</p>
            )}
          </div>

          <div>
            <label className="text-sm font-medium">Apellido</label>
            <input
              type="text"
              value={apellido}
              onChange={(e) => setApellido(e.target.value)}
              maxLength={100}
              required
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['Apellido'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['Apellido']}</p>
            )}
          </div>

          <div>
            <label className="text-sm font-medium">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              maxLength={150}
              required
              disabled={isEditing}
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm disabled:opacity-50"
            />
            {fieldErrors['Email'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['Email']}</p>
            )}
          </div>

          {!isEditing && (
            <div>
              <label className="text-sm font-medium">Contraseña</label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                minLength={8}
                required
                className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
              />
              {fieldErrors['Password'] && (
                <p className="mt-1 text-xs text-destructive">{fieldErrors['Password']}</p>
              )}
            </div>
          )}

          <div>
            <label className="text-sm font-medium">Teléfono</label>
            <input
              type="text"
              value={telefono}
              onChange={(e) => setTelefono(e.target.value)}
              maxLength={20}
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>

          <div>
            <label className="text-sm font-medium">Número de documento</label>
            <input
              type="text"
              value={numeroDocumento}
              onChange={(e) => setNumeroDocumento(e.target.value)}
              maxLength={30}
              required
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['NumeroDocumento'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['NumeroDocumento']}</p>
            )}
          </div>

          {!isEditing && (
            <div>
              <label className="text-sm font-medium">Rol</label>
              <select
                value={rolId}
                onChange={(e) => setRolId(e.target.value)}
                required
                className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
              >
                <option value="">Seleccionar rol...</option>
                {roles?.map((rol) => (
                  <option key={rol.id} value={rol.id}>{rol.nombre}</option>
                ))}
              </select>
              {fieldErrors['RolId'] && (
                <p className="mt-1 text-xs text-destructive">{fieldErrors['RolId']}</p>
              )}
            </div>
          )}

          <div className="flex justify-end gap-3 pt-2">
            <button
              type="button"
              onClick={onClose}
              className="rounded-md border border-input bg-background px-4 py-2 text-sm font-medium hover:bg-muted"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={createUsuario.isPending || updateUsuario.isPending}
              className="rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90 disabled:opacity-50"
            >
              {createUsuario.isPending || updateUsuario.isPending
                ? 'Guardando...'
                : isEditing
                  ? 'Actualizar'
                  : 'Crear'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}