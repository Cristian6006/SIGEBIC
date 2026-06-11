import { useState } from 'react';
import { BookOpen, CheckCircle, RefreshCw, AlertTriangle } from 'lucide-react';
//import { useAuth } from '@/hooks/useAuth';
import {
  usePrestamos,
  usePrestamosVencidos,
  useRegistrarDevolucion,
  useRenovarPrestamo,
} from '@/hooks/usePrestamos';
import type { PrestamoDto, PrestamosFiltros } from '@/types/prestamo.types';
import RegistrarPrestamoModal from './RegistrarPrestamoModal';

export default function PrestamosActivosPage() {
  //const { usuario } = useAuth();

  // Filtros
  const [filtros, setFiltros] = useState<PrestamosFiltros>({
    pagina: 1,
    tamanoPagina: 10,
  });
  const [buscarTexto, setBuscarTexto] = useState('');

  // Modal de nuevo préstamo
  const [modalOpen, setModalOpen] = useState(false);

  // Queries
  const { data, isLoading, isError } = usePrestamos(filtros);
  const { data: vencidos } = usePrestamosVencidos();

  // Mutations
  const devolucion = useRegistrarDevolucion();
  const renovar = useRenovarPrestamo();

  async function handleDevolver(prestamo: PrestamoDto) {
    if (confirm(`¿Registrar devolución de "${prestamo.tituloLibro}"?`)) {
      await devolucion.mutateAsync({ id: prestamo.id });
    }
  }

  async function handleRenovar(prestamo: PrestamoDto) {
    const dias = prompt('Días de extensión (1-15):', '7');
    if (dias) {
      const diasNum = parseInt(dias, 10);
      if (diasNum >= 1 && diasNum <= 15) {
        await renovar.mutateAsync({ id: prestamo.id, diasExtension: diasNum });
      } else {
        alert('Los días deben estar entre 1 y 15.');
      }
    }
  }

  function estadoBadge(estado: string) {
    switch (estado) {
      case 'Activo':
        return (
          <span className="inline-flex items-center rounded-full bg-blue-100 px-2.5 py-0.5 text-xs font-medium text-blue-800">
            Activo
          </span>
        );
      case 'Devuelto':
        return (
          <span className="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-medium text-green-800">
            Devuelto
          </span>
        );
      case 'Vencido':
        return (
          <span className="inline-flex items-center rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-medium text-red-800">
            Vencido
          </span>
        );
      case 'Renovado':
        return (
          <span className="inline-flex items-center rounded-full bg-yellow-100 px-2.5 py-0.5 text-xs font-medium text-yellow-800">
            Renovado
          </span>
        );
      default:
        return <span>{estado}</span>;
    }
  }

  function rowClass(prestamo: PrestamoDto) {
    if (prestamo.estaVencido) return 'bg-red-50 hover:bg-red-100/50';
    if (prestamo.diasRestantes <= 2 && prestamo.estado === 'Activo') return 'bg-yellow-50 hover:bg-yellow-100/50';
    return 'hover:bg-muted/30';
  }

  function formatDate(dateStr: string) {
    return new Date(dateStr).toLocaleDateString('es-CO', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold text-black">Préstamos</h1>
          <p className="text-sm text-muted-foreground">
            Gestiona los préstamos activos de la biblioteca.
          </p>
        </div>
        <button
          onClick={() => setModalOpen(true)}
          className="inline-flex items-center gap-2 rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90"
        >
          <BookOpen className="h-4 w-4" />
          Nuevo préstamo
        </button>
      </div>

      {/* Alerta de vencidos */}
      {vencidos && vencidos.length > 0 && (
        <div className="rounded-lg border border-red-200 bg-red-50 p-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <AlertTriangle className="h-5 w-5 text-red-600" />
              <p className="text-sm font-medium text-red-800">
                Hay {vencidos.length} préstamo{vencidos.length !== 1 ? 's' : ''} vencido{vencidos.length !== 1 ? 's' : ''}
              </p>
            </div>
            <button
              onClick={() => setFiltros((prev) => ({ ...prev, vencidos: true, pagina: 1 }))}
              className="text-sm text-red-700 underline hover:text-red-900"
            >
              Ver solo vencidos
            </button>
          </div>
        </div>
      )}

      {/* Filtros */}
      <div className="rounded-lg border bg-card p-4">
        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
          <div>
            <label className="text-sm font-medium">Buscar</label>
            <input
              type="text"
              value={buscarTexto}
              onChange={(e) => setBuscarTexto(e.target.value)}
              placeholder="Buscar por usuario o libro..."
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
              onKeyDown={(e) => {
                if (e.key === 'Enter') {
                  setFiltros((prev) => ({
                    ...prev,
                    pagina: 1,
                    // The search is done client-side for now
                  }));
                }
              }}
            />
          </div>
          <div>
            <label className="text-sm font-medium">Estado</label>
            <select
              value={filtros.estado || ''}
              onChange={(e) =>
                setFiltros((prev) => ({
                  ...prev,
                  estado: e.target.value || undefined,
                  pagina: 1,
                }))
              }
              className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            >
              <option value="">Todos</option>
              <option value="Activo">Activo</option>
              <option value="Devuelto">Devuelto</option>
              <option value="Vencido">Vencido</option>
              <option value="Renovado">Renovado</option>
            </select>
          </div>
          <div className="flex items-end pb-2">
            <label className="flex items-center gap-2 text-sm font-medium">
              <input
                type="checkbox"
                checked={filtros.vencidos || false}
                onChange={(e) =>
                  setFiltros((prev) => ({
                    ...prev,
                    vencidos: e.target.checked || undefined,
                    pagina: 1,
                  }))
                }
                className="h-4 w-4 rounded border-input"
              />
              Solo vencidos
            </label>
          </div>
          <div className="flex items-end">
            <button
              onClick={() =>
                setFiltros((prev) => ({
                  ...prev,
                  pagina: 1,
                  libroId: undefined,
                  usuarioId: undefined,
                }))
              }
              className="w-full rounded-md bg-secondary px-4 py-2 text-sm font-medium text-secondary-foreground hover:bg-secondary/80"
            >
              Limpiar filtros
            </button>
          </div>
        </div>
      </div>

      {/* Tabla */}
      {isLoading && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-muted-foreground">Cargando préstamos...</p>
        </div>
      )}

      {isError && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-destructive">Ocurrió un error al cargar los préstamos.</p>
        </div>
      )}

      {data && data.items.length === 0 && (
        <div className="flex flex-col items-center py-12 text-center">
          <BookOpen className="h-12 w-12 text-muted-foreground" />
          <p className="mt-4 text-sm text-muted-foreground">
            No se encontraron préstamos con los filtros seleccionados.
          </p>
        </div>
      )}

      {data && data.items.length > 0 && (
        <div className="overflow-x-auto rounded-lg border">
          <table className="w-full text-sm">
            <thead className="bg-muted/50">
              <tr>
                <th className="px-4 py-3 text-left font-medium">Usuario</th>
                <th className="px-4 py-3 text-left font-medium">Libro</th>
                <th className="px-4 py-3 text-left font-medium hidden md:table-cell">
                  Fecha préstamo
                </th>
                <th className="px-4 py-3 text-left font-medium">Vence</th>
                <th className="px-4 py-3 text-center font-medium hidden sm:table-cell">
                  Días rest.
                </th>
                <th className="px-4 py-3 text-center font-medium">Estado</th>
                <th className="px-4 py-3 text-center font-medium hidden sm:table-cell">
                  Renov.
                </th>
                <th className="px-4 py-3 text-right font-medium">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y">
              {data.items.map((prestamo) => (
                <tr key={prestamo.id} className={rowClass(prestamo)}>
                  <td className="px-4 py-3 font-medium">{prestamo.nombreUsuario}</td>
                  <td className="px-4 py-3">{prestamo.tituloLibro}</td>
                  <td className="px-4 py-3 hidden md:table-cell">
                    {formatDate(prestamo.fechaPrestamo)}
                  </td>
                  <td className="px-4 py-3">{formatDate(prestamo.fechaDevolucionEsperada)}</td>
                  <td className="px-4 py-3 text-center hidden sm:table-cell">
                    <span
                      className={
                        prestamo.estaVencido
                          ? 'font-medium text-red-600'
                          : prestamo.diasRestantes <= 2
                          ? 'font-medium text-yellow-600'
                          : 'text-muted-foreground'
                      }
                    >
                      {prestamo.estaVencido ? 'Vencido' : prestamo.diasRestantes}
                    </span>
                  </td>
                  <td className="px-4 py-3 text-center">{estadoBadge(prestamo.estado)}</td>
                  <td className="px-4 py-3 text-center hidden sm:table-cell">
                    {prestamo.cantidadRenovaciones}
                  </td>
                  <td className="px-4 py-3 text-right">
                    {(prestamo.estado === 'Activo' || prestamo.estado === 'Renovado') && (
                      <div className="flex items-center justify-end gap-1">
                        <button
                          onClick={() => handleDevolver(prestamo)}
                          disabled={devolucion.isPending}
                          className="rounded-md p-1.5 text-green-600 hover:bg-green-50 hover:text-green-700"
                          title="Registrar devolución"
                        >
                          <CheckCircle className="h-4 w-4" />
                        </button>
                        <button
                          onClick={() => handleRenovar(prestamo)}
                          disabled={renovar.isPending || prestamo.cantidadRenovaciones >= 2}
                          className="rounded-md p-1.5 text-blue-600 hover:bg-blue-50 hover:text-blue-700"
                          title="Renovar"
                        >
                          <RefreshCw className="h-4 w-4" />
                        </button>
                      </div>
                    )}
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
            Página {data.paginaActual} de {data.totalPaginas} — {data.totalRegistros} registros
          </p>
          <div className="flex gap-2">
            <button
              onClick={() =>
                setFiltros((prev) => ({ ...prev, pagina: prev.pagina - 1 }))
              }
              disabled={!data.tienePaginaAnterior}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              Anterior
            </button>
            <button
              onClick={() =>
                setFiltros((prev) => ({ ...prev, pagina: prev.pagina + 1 }))
              }
              disabled={!data.tienePaginaSiguiente}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              Siguiente
            </button>
          </div>
        </div>
      )}

      {/* Modal nuevo préstamo */}
      <RegistrarPrestamoModal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
      />
    </div>
  );
}