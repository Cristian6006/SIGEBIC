import { useState } from 'react';
import { BookOpen, Pencil, Trash2, History } from 'lucide-react';
import { useAuth } from '@/hooks/useAuth';
import { useLibros, useDarDeBajaLibro } from '@/hooks/useLibros';
import type { LibroDto, LibrosFiltros } from '@/types/libro.types';
import LibroFormModal from './LibroFormModal';
import HistorialLibroModal from '@/pages/prestamos/HistorialLibroModal';

type Rol = 'Administrador' | 'Bibliotecario' | 'Lector';

export default function CatalogoPage() {
  const { usuario } = useAuth();
  const rol = (usuario?.rol || 'Lector') as Rol;

  // Filtros locales (no se aplican hasta presionar "Buscar")
  const [titulo, setTitulo] = useState('');
  const [autor, setAutor] = useState('');
  const [genero, setGenero] = useState('');
  const [soloDisponibles, setSoloDisponibles] = useState(false);

  // Filtros aplicados (los que realmente se envían al backend)
  const [filtros, setFiltros] = useState<LibrosFiltros>({
    pagina: 1,
    tamanoPagina: 10,
  });

  const { data, isLoading, isError } = useLibros(filtros);

  // Modal crear/editar
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedLibro, setSelectedLibro] = useState<LibroDto | null>(null);

  // Modal historial
  const [historialModalOpen, setHistorialModalOpen] = useState(false);
  const [historialLibroId, setHistorialLibroId] = useState<string | null>(null);
  const [historialTitulo, setHistorialTitulo] = useState('');

  // Dar de baja
  const darDeBaja = useDarDeBajaLibro();

  function handleBuscar() {
    setFiltros((prev) => ({
      ...prev,
      pagina: 1,
      titulo: titulo || undefined,
      autor: autor || undefined,
      genero: genero || undefined,
      soloDisponibles: soloDisponibles || undefined,
    }));
  }

  function handlePaginaAnterior() {
    if (data?.tienePaginaAnterior) {
      setFiltros((prev) => ({ ...prev, pagina: prev.pagina - 1 }));
    }
  }

  function handlePaginaSiguiente() {
    if (data?.tienePaginaSiguiente) {
      setFiltros((prev) => ({ ...prev, pagina: prev.pagina + 1 }));
    }
  }

  function handleNuevoLibro() {
    setSelectedLibro(null);
    setModalOpen(true);
  }

  function handleEditarLibro(libro: LibroDto) {
    setSelectedLibro(libro);
    setModalOpen(true);
  }

  function handleVerHistorial(libro: LibroDto) {
    setHistorialLibroId(libro.id);
    setHistorialTitulo(libro.titulo);
    setHistorialModalOpen(true);
  }

  async function handleDarDeBaja(id: string) {
    if (confirm('¿Estás seguro de dar de baja este libro?')) {
      await darDeBaja.mutateAsync(id);
    }
  }

  function estadoBadge(estado: string) {
    switch (estado) {
      case 'Disponible':
        return (
          <span className="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-medium text-green-800">
            Disponible
          </span>
        );
      case 'Prestado':
        return (
          <span className="inline-flex items-center rounded-full bg-yellow-100 px-2.5 py-0.5 text-xs font-medium text-yellow-800">
            Prestado
          </span>
        );
      case 'Perdido':
        return (
          <span className="inline-flex items-center rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-medium text-red-800">
            Perdido
          </span>
        );
      case 'DeBaja':
        return (
          <span className="inline-flex items-center rounded-full bg-gray-100 px-2.5 py-0.5 text-xs font-medium text-gray-800">
            De baja
          </span>
        );
      default:
        return <span>{estado}</span>;
    }
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-black">Catálogo</h1>
          <p className="text-sm text-muted-foreground">
            Consulta y gestiona los libros de la biblioteca.
          </p>
        </div>

        {(rol === 'Administrador' || rol === 'Bibliotecario') && (
          <button
            onClick={handleNuevoLibro}
            className="inline-flex items-center gap-2 rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90"
          >
            <BookOpen className="h-4 w-4" />
            Nuevo libro
          </button>
        )}
      </div>

      {/* Filtros */}
      <div className="rounded-lg border bg-card p-4">
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-5">
          <div>
            <label className="text-sm font-medium">Título</label>
            <input
              type="text"
              value={titulo}
              onChange={(e) => setTitulo(e.target.value)}
              placeholder="Buscar por título..."
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>
          <div>
            <label className="text-sm font-medium">Autor</label>
            <input
              type="text"
              value={autor}
              onChange={(e) => setAutor(e.target.value)}
              placeholder="Buscar por autor..."
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>
          <div>
            <label className="text-sm font-medium">Género</label>
            <input
              type="text"
              value={genero}
              onChange={(e) => setGenero(e.target.value)}
              placeholder="Buscar por género..."
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>
          <div className="flex items-end pb-2">
            <label className="flex items-center gap-2 text-sm font-medium">
              <input
                type="checkbox"
                checked={soloDisponibles}
                onChange={(e) => setSoloDisponibles(e.target.checked)}
                className="h-4 w-4 rounded border-input"
              />
              Solo disponibles
            </label>
          </div>
          <div className="flex items-end">
            <button
              onClick={handleBuscar}
              className="w-full rounded-md bg-secondary px-4 py-2 text-sm font-medium text-secondary-foreground hover:bg-secondary/80"
            >
              Buscar
            </button>
          </div>
        </div>
      </div>

      {/* Tabla */}
      {isLoading && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-muted-foreground">Cargando libros...</p>
        </div>
      )}

      {isError && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-destructive">
            Ocurrió un error al cargar los libros.
          </p>
        </div>
      )}

      {data && data.items.length === 0 && (
        <div className="flex flex-col items-center py-12 text-center">
          <BookOpen className="h-12 w-12 text-muted-foreground" />
          <p className="mt-4 text-sm text-muted-foreground">
            No se encontraron libros con los filtros seleccionados.
          </p>
        </div>
      )}

      {data && data.items.length > 0 && (
        <div className="overflow-x-auto rounded-lg border">
          <table className="w-full text-sm">
            <thead className="bg-muted/50">
              <tr>
                <th className="px-4 py-3 text-left font-medium">Título</th>
                <th className="px-4 py-3 text-left font-medium">Autor</th>
                <th className="px-4 py-3 text-left font-medium hidden md:table-cell">
                  Género
                </th>
                <th className="px-4 py-3 text-center font-medium hidden sm:table-cell">
                  Disponibles
                </th>
                <th className="px-4 py-3 text-center font-medium">Estado</th>
                <th className="px-4 py-3 text-right font-medium">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y">
              {data.items.map((libro) => (
                <tr key={libro.id} className="hover:bg-muted/30">
                  <td className="px-4 py-3 font-medium">{libro.titulo}</td>
                  <td className="px-4 py-3">{libro.autor}</td>
                  <td className="px-4 py-3 hidden md:table-cell">
                    {libro.genero || '—'}
                  </td>
                  <td className="px-4 py-3 text-center hidden sm:table-cell">
                    {libro.cantidadDisponible}/{libro.cantidadTotal}
                  </td>
                  <td className="px-4 py-3 text-center">
                    {estadoBadge(libro.estado)}
                  </td>
                  <td className="px-4 py-3 text-right">
                    <div className="flex items-center justify-end gap-1">
                      <button
                        onClick={() => handleVerHistorial(libro)}
                        className="rounded-md p-1.5 text-muted-foreground hover:bg-muted hover:text-foreground"
                        title="Ver historial"
                      >
                        <History className="h-4 w-4" />
                      </button>
                      {(rol === 'Administrador' || rol === 'Bibliotecario') && (
                        <button
                          onClick={() => handleEditarLibro(libro)}
                          className="rounded-md p-1.5 text-muted-foreground hover:bg-muted hover:text-foreground"
                          title="Editar"
                        >
                          <Pencil className="h-4 w-4" />
                        </button>
                      )}
                      {rol === 'Administrador' && (
                        <button
                          onClick={() => handleDarDeBaja(libro.id)}
                          disabled={darDeBaja.isPending}
                          className="rounded-md p-1.5 text-muted-foreground hover:bg-destructive/10 hover:text-destructive"
                          title="Dar de baja"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Paginador */}
      {data && data.totalPaginas > 0 && (
        <div className="flex items-center justify-between">
          <p className="text-sm text-muted-foreground">
            Página {data.paginaActual} de {data.totalPaginas} —{' '}
            {data.totalRegistros} registros
          </p>
          <div className="flex gap-2">
            <button
              onClick={handlePaginaAnterior}
              disabled={!data.tienePaginaAnterior}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              Anterior
            </button>
            <button
              onClick={handlePaginaSiguiente}
              disabled={!data.tienePaginaSiguiente}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              Siguiente
            </button>
          </div>
        </div>
      )}

      {/* Modal crear/editar */}
      <LibroFormModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        libro={selectedLibro}
      />

      {/* Modal historial */}
      <HistorialLibroModal
        open={historialModalOpen}
        onClose={() => setHistorialModalOpen(false)}
        libroId={historialLibroId}
        tituloLibro={historialTitulo}
      />
    </div>
  );
}