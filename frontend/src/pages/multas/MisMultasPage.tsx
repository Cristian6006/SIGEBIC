import { useState } from 'react';
import { AlertTriangle, DollarSign } from 'lucide-react';
import { useMisMultas } from '@/hooks/useMultas';

export default function MisMultasPage() {
  const [pagina, setPagina] = useState(1);

  const { data, isLoading, isError } = useMisMultas(pagina, 10);

  function formatDate(dateStr: string) {
    return new Date(dateStr).toLocaleDateString('es-CO', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }

  function formatCOP(amount: number) {
    return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', minimumFractionDigits: 0 }).format(amount);
  }

  const multasPendientes = data?.items.filter((m) => !m.pagada) ?? [];
  const totalPendiente = multasPendientes.reduce((sum, m) => sum + m.montoTotal, 0);

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold text-black">Mis Multas</h1>
        <p className="text-sm text-muted-foreground">
          Consulta y gestiona tus multas pendientes y pagadas.
        </p>
      </div>

      {/* Card resumen */}
      <div className="rounded-lg border bg-card p-6">
        <div className="flex items-center gap-3">
          <div className="rounded-full bg-amber-100 p-3">
            <DollarSign className="h-6 w-6 text-amber-600" />
          </div>
          <div>
            <p className="text-sm text-muted-foreground">
              Multas pendientes: <strong>{multasPendientes.length}</strong>
            </p>
            <p className="text-sm text-muted-foreground">
              Total a pagar: <span className="font-bold text-red-600">{formatCOP(totalPendiente)}</span>
            </p>
          </div>
        </div>
      </div>

      {/* Loading */}
      {isLoading && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-muted-foreground">Cargando multas...</p>
        </div>
      )}

      {/* Error */}
      {isError && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-destructive">Ocurrió un error al cargar las multas.</p>
        </div>
      )}

      {/* Vacío */}
      {!isLoading && !isError && data && data.items.length === 0 && (
        <div className="flex flex-col items-center py-12 text-center">
          <AlertTriangle className="h-12 w-12 text-green-500" />
          <p className="mt-4 text-sm font-medium text-green-700">
            No tienes multas registradas.
          </p>
          <p className="text-sm text-muted-foreground">
            Las multas se generan automáticamente al devolver libros con retraso.
          </p>
        </div>
      )}

      {/* Tabla */}
      {data && data.items.length > 0 && (
        <div className="overflow-x-auto rounded-lg border">
          <table className="w-full text-sm">
            <thead className="bg-muted/50">
              <tr>
                <th className="px-4 py-3 text-left font-medium">Libro</th>
                <th className="px-4 py-3 text-center font-medium">Días retraso</th>
                <th className="px-4 py-3 text-right font-medium">Monto</th>
                <th className="px-4 py-3 text-center font-medium">Estado</th>
                <th className="px-4 py-3 text-left font-medium hidden md:table-cell">
                  Fecha generación
                </th>
                <th className="px-4 py-3 text-left font-medium hidden md:table-cell">
                  Fecha pago
                </th>
              </tr>
            </thead>
            <tbody className="divide-y">
              {data.items.map((multa) => (
                <tr key={multa.id} className="hover:bg-muted/30">
                  <td className="px-4 py-3 font-medium">{multa.tituloLibro}</td>
                  <td className="px-4 py-3 text-center">
                    <span className="font-medium text-red-600">{multa.diasRetraso} días</span>
                  </td>
                  <td className="px-4 py-3 text-right font-semibold">
                    {formatCOP(multa.montoTotal)}
                  </td>
                  <td className="px-4 py-3 text-center">
                    {multa.pagada ? (
                      <span className="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-medium text-green-800">
                        Pagada
                      </span>
                    ) : (
                      <span className="inline-flex items-center rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-medium text-red-800">
                        Pendiente
                      </span>
                    )}
                  </td>
                  <td className="px-4 py-3 hidden md:table-cell">
                    {formatDate(multa.fechaGeneracion)}
                  </td>
                  <td className="px-4 py-3 hidden md:table-cell">
                    {multa.fechaPago ? formatDate(multa.fechaPago) : '-'}
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
              onClick={() => setPagina((p) => p - 1)}
              disabled={!data.tienePaginaAnterior}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              Anterior
            </button>
            <button
              onClick={() => setPagina((p) => p + 1)}
              disabled={!data.tienePaginaSiguiente}
              className="rounded-md border border-input bg-background px-3 py-1.5 text-sm font-medium hover:bg-muted disabled:opacity-50"
            >
              Siguiente
            </button>
          </div>
        </div>
      )}
    </div>
  );
}