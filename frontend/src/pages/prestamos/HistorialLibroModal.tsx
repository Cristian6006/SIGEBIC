import { useState } from 'react';
import { X } from 'lucide-react';
import { useHistorialByLibro } from '@/hooks/usePrestamos';

interface HistorialLibroModalProps {
  open: boolean;
  onClose: () => void;
  libroId: string | null;
  tituloLibro: string;
}

export default function HistorialLibroModal({ open, onClose, libroId, tituloLibro }: HistorialLibroModalProps) {
  const [pagina, setPagina] = useState(1);
  const { data, isLoading } = useHistorialByLibro(libroId, pagina);

  function formatDate(dateStr: string) {
    return new Date(dateStr).toLocaleDateString('es-CO', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  }

  function estadoBadge(estado: string) {
    switch (estado) {
      case 'Devuelto':
        return (
          <span className="inline-flex items-center rounded-full bg-green-100 px-2.5 py-0.5 text-xs font-medium text-green-800">
            A tiempo
          </span>
        );
      case 'Vencido':
        return (
          <span className="inline-flex items-center rounded-full bg-red-100 px-2.5 py-0.5 text-xs font-medium text-red-800">
            Vencido
          </span>
        );
      default:
        return (
          <span className="inline-flex items-center rounded-full bg-yellow-100 px-2.5 py-0.5 text-xs font-medium text-yellow-800">
            {estado}
          </span>
        );
    }
  }

  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-2xl rounded-lg bg-white p-6 shadow-xl">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-lg font-semibold text-black">Historial del libro</h2>
            <p className="text-sm text-muted-foreground">{tituloLibro}</p>
          </div>
          <button
            onClick={onClose}
            className="rounded-md p-1.5 text-muted-foreground hover:bg-muted"
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        {isLoading && (
          <div className="flex justify-center py-8">
            <p className="text-sm text-muted-foreground">Cargando historial...</p>
          </div>
        )}

        {data && data.items.length === 0 && (
          <p className="py-8 text-center text-sm text-muted-foreground">
            Este libro no tiene historial de préstamos.
          </p>
        )}

        {data && data.items.length > 0 && (
          <>
            <div className="mt-4 overflow-x-auto rounded-lg border">
              <table className="w-full text-sm">
                <thead className="bg-muted/50">
                  <tr>
                    <th className="px-4 py-3 text-left font-medium">Usuario</th>
                    <th className="px-4 py-3 text-left font-medium hidden sm:table-cell">
                      Fecha préstamo
                    </th>
                    <th className="px-4 py-3 text-left font-medium">Devuelto</th>
                    <th className="px-4 py-3 text-center font-medium">Estado</th>
                    <th className="px-4 py-3 text-center font-medium">Días retraso</th>
                  </tr>
                </thead>
                <tbody className="divide-y">
                  {data.items.map((h) => (
                    <tr key={h.id} className="hover:bg-muted/30">
                      <td className="px-4 py-3 font-medium">{h.nombreUsuario}</td>
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

            {data.totalPaginas > 0 && (
              <div className="mt-4 flex items-center justify-between">
                <p className="text-sm text-muted-foreground">
                  Página {data.paginaActual} de {data.totalPaginas}
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
          </>
        )}
      </div>
    </div>
  );
}