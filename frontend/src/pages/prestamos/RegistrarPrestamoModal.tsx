import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Search } from 'lucide-react';
import { useAuth } from '@/hooks/useAuth';
import { useRegistrarPrestamo } from '@/hooks/usePrestamos';
import { getLibros } from '@/api/libros.api';
import { getUsuarios } from '@/api/usuarios.api';
import type { LibroDto } from '@/types/libro.types';
import type { UsuarioDto } from '@/types/usuario.types';

interface RegistrarPrestamoModalProps {
  open: boolean;
  onClose: () => void;
}

export default function RegistrarPrestamoModal({ open, onClose }: RegistrarPrestamoModalProps) {
  const { usuario } = useAuth();
  const esAdmin = usuario?.rol === 'Administrador' || usuario?.rol === 'Bibliotecario';

  const [usuarioSearch, setUsuarioSearch] = useState('');
  const [selectedUsuario, setSelectedUsuario] = useState<UsuarioDto | null>(null);
  const [libroSearch, setLibroSearch] = useState('');
  const [selectedLibro, setSelectedLibro] = useState<LibroDto | null>(null);
  const [diasPrestamo, setDiasPrestamo] = useState(14);
  const [observaciones, setObservaciones] = useState('');
  const [error, setError] = useState<string | null>(null);

  const registrarPrestamo = useRegistrarPrestamo();

  // Debounced usuario search
  const { data: usuariosData } = useQuery({
    queryKey: ['usuarios', 'search', usuarioSearch],
    queryFn: () =>
      getUsuarios({ nombre: usuarioSearch || undefined, pagina: 1, tamanoPagina: 10 }),
    enabled: open && esAdmin && usuarioSearch.length >= 2,
  });

  // Debounced libro search (only available ones)
  const { data: librosData } = useQuery({
    queryKey: ['libros', 'search', libroSearch],
    queryFn: () =>
      getLibros({ titulo: libroSearch || undefined, soloDisponibles: true, pagina: 1, tamanoPagina: 10 }),
    enabled: open && libroSearch.length >= 2,
  });

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);

    if (!selectedUsuario) {
      setError('Debes seleccionar un usuario.');
      return;
    }
    if (!selectedLibro) {
      setError('Debes seleccionar un libro.');
      return;
    }
    if (diasPrestamo < 1 || diasPrestamo > 30) {
      setError('Los días de préstamo deben estar entre 1 y 30.');
      return;
    }

    try {
      await registrarPrestamo.mutateAsync({
        usuarioId: selectedUsuario.id,
        libroId: selectedLibro.id,
        diasPrestamo,
        observaciones: observaciones || undefined,
      });
      handleClose();
    } catch (err: any) {
      const serverError = err?.response?.data;
      if (serverError?.errors) {
        const messages = Object.values(serverError.errors).flat().join('. ');
        setError(messages);
      } else if (serverError?.title) {
        setError(serverError.title);
      } else {
        setError('Ocurrió un error al registrar el préstamo.');
      }
    }
  }

  function handleClose() {
    setUsuarioSearch('');
    setSelectedUsuario(null);
    setLibroSearch('');
    setSelectedLibro(null);
    setDiasPrestamo(14);
    setObservaciones('');
    setError(null);
    onClose();
  }

  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-lg rounded-lg bg-white p-6 shadow-xl">
        <h2 className="text-lg font-semibold text-black">Nuevo Préstamo</h2>
        <p className="mt-1 text-sm text-muted-foreground">
          Registra un nuevo préstamo de libro.
        </p>

        <form onSubmit={handleSubmit} className="mt-4 space-y-4">
          {error && (
            <div className="rounded-md bg-red-50 p-3 text-sm text-red-800">{error}</div>
          )}

          {/* Selección de usuario */}
          <div>
            <label className="text-sm font-medium">Usuario</label>
            {selectedUsuario ? (
              <div className="mt-1 flex items-center justify-between rounded-md border border-input bg-background px-3 py-2">
                    <span className="text-sm">{selectedUsuario.nombre} {selectedUsuario.apellido}</span>
                <button
                  type="button"
                  onClick={() => setSelectedUsuario(null)}
                  className="text-xs text-destructive hover:underline"
                >
                  Cambiar
                </button>
              </div>
            ) : (
              <div className="relative mt-1">
                <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                <input
                  type="text"
                  value={usuarioSearch}
                  onChange={(e) => setUsuarioSearch(e.target.value)}
                  placeholder="Buscar usuario por nombre..."
                  className="w-full rounded-md border border-input bg-background pl-9 pr-3 py-2 text-sm"
                />
                {usuariosData && usuariosData.items.length > 0 && (
                  <div className="absolute z-10 mt-1 max-h-40 w-full overflow-y-auto rounded-md border bg-white shadow-lg">
                    {usuariosData.items.map((u: UsuarioDto) => (
                      <button
                        key={u.id}
                        type="button"
                        onClick={() => {
                          setSelectedUsuario(u);
                          setUsuarioSearch('');
                        }}
                        className="w-full px-3 py-2 text-left text-sm hover:bg-muted"
                      >
                      {u.nombre} {u.apellido} — {u.email}
                      </button>
                    ))}
                  </div>
                )}
              </div>
            )}
          </div>

          {/* Selección de libro */}
          <div>
            <label className="text-sm font-medium">Libro</label>
            {selectedLibro ? (
              <div className="mt-1 flex items-center justify-between rounded-md border border-input bg-background px-3 py-2">
                <span className="text-sm">
                  {selectedLibro.titulo} — {selectedLibro.autor}
                </span>
                <button
                  type="button"
                  onClick={() => setSelectedLibro(null)}
                  className="text-xs text-destructive hover:underline"
                >
                  Cambiar
                </button>
              </div>
            ) : (
              <div className="relative mt-1">
                <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                <input
                  type="text"
                  value={libroSearch}
                  onChange={(e) => setLibroSearch(e.target.value)}
                  placeholder="Buscar libro por título..."
                  className="w-full rounded-md border border-input bg-background pl-9 pr-3 py-2 text-sm"
                />
                {librosData && librosData.items.length > 0 && (
                  <div className="absolute z-10 mt-1 max-h-40 w-full overflow-y-auto rounded-md border bg-white shadow-lg">
                    {librosData.items.map((l: LibroDto) => (
                      <button
                        key={l.id}
                        type="button"
                        onClick={() => {
                          setSelectedLibro(l);
                          setLibroSearch('');
                        }}
                        className="w-full px-3 py-2 text-left text-sm hover:bg-muted"
                      >
                        {l.titulo} — {l.autor}
                      </button>
                    ))}
                  </div>
                )}
              </div>
            )}
          </div>

          {/* Días de préstamo */}
          <div>
            <label className="text-sm font-medium">Días de préstamo</label>
            <input
              type="number"
              value={diasPrestamo}
              onChange={(e) => setDiasPrestamo(Number(e.target.value))}
              min={1}
              max={30}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>

          {/* Observaciones */}
          <div>
            <label className="text-sm font-medium">Observaciones (opcional)</label>
            <textarea
              value={observaciones}
              onChange={(e) => setObservaciones(e.target.value)}
              rows={2}
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>

          {/* Botones */}
          <div className="flex justify-end gap-3">
            <button
              type="button"
              onClick={handleClose}
              className="rounded-md border border-input bg-background px-4 py-2 text-sm font-medium hover:bg-muted"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={registrarPrestamo.isPending}
              className="rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90 disabled:opacity-50"
            >
              {registrarPrestamo.isPending ? 'Registrando...' : 'Registrar préstamo'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}