import { useState, useEffect } from 'react';
import { X } from 'lucide-react';
import { useAsignarRol, useRoles } from '@/hooks/useUsuarios';
import type { UsuarioDto } from '@/types/usuario.types';

interface AsignarRolModalProps {
  open: boolean;
  onClose: () => void;
  usuario: UsuarioDto | null;
}

export default function AsignarRolModal({ open, onClose, usuario }: AsignarRolModalProps) {
  const [rolId, setRolId] = useState('');

  const { data: roles } = useRoles();
  const asignarRol = useAsignarRol();

  useEffect(() => {
    if (usuario) {
      setRolId(usuario.rolId);
    }
  }, [usuario, open]);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!usuario || !rolId) return;

    await asignarRol.mutateAsync({ id: usuario.id, rolId });
    onClose();
  }

  if (!open || !usuario) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
      <div className="w-full max-w-md rounded-lg bg-card p-6 shadow-lg">
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-lg font-semibold">Cambiar rol</h2>
          <button
            onClick={onClose}
            className="rounded-md p-1 hover:bg-muted"
            type="button"
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        <p className="text-sm text-muted-foreground mb-4">
          Usuario: <span className="font-medium text-foreground">{usuario.nombre} {usuario.apellido}</span>
          <br />
          Rol actual: <span className="font-medium text-foreground">{usuario.nombreRol}</span>
        </p>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="text-sm font-medium">Nuevo rol</label>
            <select
              value={rolId}
              onChange={(e) => setRolId(e.target.value)}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            >
              {roles?.map((rol) => (
                <option key={rol.id} value={rol.id}>{rol.nombre}</option>
              ))}
            </select>
          </div>

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
              disabled={asignarRol.isPending}
              className="rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90 disabled:opacity-50"
            >
              {asignarRol.isPending ? 'Guardando...' : 'Cambiar rol'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}