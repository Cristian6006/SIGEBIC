import { useState } from 'react';
import { BookOpen, History } from 'lucide-react';
import { useMisPrestamos, useMiHistorial } from '@/hooks/usePrestamos';

export default function MisPrestamosPage() {
  const [paginaPrestamos, setPaginaPrestamos] = useState(1);
  const [paginaHistorial, setPaginaHistorial] = useState(1);

  const { data: prestamos, isLoading: loadingPrestamos } = useMisPrestamos(paginaPrestamos, 10);
  const { data: historial, isLoading: loadingHistorial } = useMiHistorial(paginaHistorial);

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

  function formatDate(dateStr: string) {
    return new Date(dateStr).toLocaleDateString('es-CO', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold text-black">Mis Préstamos</h1>
        <p className="text-sm text-muted-foreground">
          Consulta tus préstamos activos e historial.
        </p>
      </div>

      {/* Préstamos activos */}
      <div>
        <h2 className="text-lg font-semibold text-black mb-3">Préstamos</h2>
        {loadingPrestamos && (
          <div className="flex justify-center py-8">
            <p className="text-sm text-muted-foreground">Cargando préstamos...</p>
          </div>
        )}

        {prestamos && prestamos.items.length === 0 && (
          <div className="flex flex-col items-center py-8 text-center">
            <BookOpen className="h-10 w-10 text-muted-foreground" />
            <p className="mt-2 text-sm text-muted-foreground">No tienes préstamos activos.</p>
          </div>
        )}

        {prestamos && prestamos.items.length > 0 && (
          <div className="overflow-x-auto rounded-lg border">
            <table className="w-full text-sm">
              <thead className="bg-muted/50">
                <tr>
                  <th className="px-4 py-3 text-left font-medium">Libro</th>
                  <th className="px-4 py-3 text-left font-medium hidden sm:table-cell">
                    Fecha préstamo
                  </th>
                  <th className="px-4 py-3 text-left font-medium">Vence</th>
                  <th className="px-4 py-3 text-center font-medium hidden sm:table-cell">
                    Días rest.
                  </th>
                  <th className="px-4 py-3 text-center font-medium">Estado</th>
                  <th className="px-4 py-3 text-center font-medium">Renov.</th>
                </tr>
              </thead>
              <tbody className="divide-y">
                {prestamos.items.map((p) => (
                  <tr
                    key={p.id}
                    className={
                      p.estaVencido
                        ? 'bg-red-50 hover:bg-red-100/50'
                        : p.diasRestantes <= 2 && p.estado === 'Activo'
                        ? 'bg-yellow-50 hover:bg-yellow-100/50'
                        : 'hover:bg-muted/30'
                    }
                  >
                    <td className="px-4 py-3 font-medium">{p.tituloLibro}</td>
                    <td className="px-4 py-3 hidden sm:table-cell">
                      {formatDate(p.fechaPrestamo)}
                    </td>
                    <td className="px-4 py-3">{formatDate(p.fechaDevolucionEsperada)}</td>
                    <td className="px-4 py-3 text-center hidden sm:table-cell">
                      <span
                        className={
                          p.estaVencido
                            ? 'font-medium text-red-600'
                            : p.diasRestantes <= 2
                            ? 'font-medium text-yellow-600'
                            : 'text-muted-foreground'
                        }
                      >
                        {p.estaVencido ? 'Vencido' : p.diasRestantes}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-center">{estadoBadge(p.estado)}</td>
                    <td className="px-4 py-3 text-center">{p.cantidadRenovaciones}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {prestamos && prestamos.totalPaginas > 0 && (
          <div className="mt-4 flex items-center justify-between">
            <p className="text-sm text-muted-foreground">
              Página {prestamos.paginaActual} de {prestamos.totalPaginas}
            </p>
            <div className="flex gap-2">
              <button
                onClick={() => setPaginaPrestamos((p) => p - 1)}
                disabled={!prestamos.tienePaginaAnterior}
                className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
              >
                Anterior
              </button>
              <button
                onClick={() => setPaginaPrestamos((p) => p + 1)}
                disabled={!prestamos.tienePaginaSiguiente}
                className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
              >
                Siguiente
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Mi historial */}
      <div className="pt-4 border-t">
        <h2 className="text-lg font-semibold text-black mb-3 flex items-center gap-2">
          <History className="h-5 w-5 text-muted-foreground" />
          Mi historial
        </h2>

        {loadingHistorial && (
          <div className="flex justify-center py-8">
            <p className="text-sm text-muted-foreground">Cargando historial...</p>
          </div>
        )}

        {historial && historial.items.length === 0 && (
          <p className="text-sm text-muted-foreground py-4">No hay historial de préstamos.</p>
        )}

        {historial && historial.items.length > 0 && (
          <div className="overflow-x-auto rounded-lg border">
            <table className="w-full text-sm">
              <thead className="bg-muted/50">
                <tr>
                  <th className="px-4 py-3 text-left font-medium">Libro</th>
                  <th className="px-4 py-3 text-left font-medium hidden sm:table-cell">
                    Fecha préstamo
                  </th>
                  <th className="px-4 py-3 text-left font-medium">Devuelto</th>
                  <th className="px-4 py-3 text-center font-medium">Estado final</th>
                  <th className="px-4 py-3 text-center font-medium">Días retraso</th>
                </tr>
              </thead>
              <tbody className="divide-y">
                {historial.items.map((h) => (
                  <tr key={h.id} className="hover:bg-muted/30">
                    <td className="px-4 py-3 font-medium">{h.tituloLibro}</td>
                    <td className="px-4 py-3 hidden sm:table-cell">
                      {formatDate(h.fechaPrestamo)}
                    </td>
                    <td className="px-4 py-3">{formatDate(h.fechaDevolucionReal)}</td>
                    <td className="px-4 py-3 text-center">{estadoBadge(h.estadoFinal)}</td>
                    <td className="px-4 py-3 text-center">
                      {h.diasRetraso > 0 ? (
                        <span className="font-medium text-red-600">{h.diasRetraso} días</span>
                      ) : (
                        <span className="text-green-600">A tiempo</span>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

        {historial && historial.totalPaginas > 0 && (
          <div className="mt-4 flex items-center justify-between">
            <p className="text-sm text-muted-foreground">
              Página {historial.paginaActual} de {historial.totalPaginas}
            </p>
            <div className="flex gap-2">
              <button
                onClick={() => setPaginaHistorial((p) => p - 1)}
                disabled={!historial.tienePaginaAnterior}
                className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
              >
                Anterior
              </button>
              <button
                onClick={() => setPaginaHistorial((p) => p + 1)}
                disabled={!historial.tienePaginaSiguiente}
                className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
              >
                Siguiente
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}