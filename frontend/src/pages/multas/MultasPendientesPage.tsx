import { useState } from 'react';
import { AlertTriangle, DollarSign, CreditCard } from 'lucide-react';
import { useMultasPendientes } from '@/hooks/useMultas';
import type { MultaDto } from '@/types/multa.types';
import RegistrarPagoModal from './RegistrarPagoModal';

export default function MultasPendientesPage() {
  const { data: multas, isLoading, isError } = useMultasPendientes();
  const [selectedMulta, setSelectedMulta] = useState<MultaDto | null>(null);
  const [pagoModalOpen, setPagoModalOpen] = useState(false);

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

  function openPagoModal(multa: MultaDto) {
    setSelectedMulta(multa);
    setPagoModalOpen(true);
  }

  function closePagoModal() {
    setSelectedMulta(null);
    setPagoModalOpen(false);
  }

  const totalPendiente = multas?.reduce((sum, m) => sum + m.montoTotal, 0) ?? 0;

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold text-black">Multas Pendientes</h1>
        <p className="text-sm text-muted-foreground">
          Panel de alertas — multas generadas automáticamente al devolver libros con retraso.
        </p>
      </div>

      {/* Card resumen */}
      <div className="rounded-lg border bg-card p-6">
        <div className="flex items-center gap-3">
          <div className="rounded-full bg-red-100 p-3">
            <DollarSign className="h-6 w-6 text-red-600" />
          </div>
          <div>
            <p className="text-sm text-muted-foreground">Total acumulado en multas pendientes</p>
            <p className="text-2xl font-bold text-red-600">{formatCOP(totalPendiente)}</p>
          </div>
        </div>
      </div>

      {/* Alerta si no hay multas */}
      {!isLoading && !isError && multas && multas.length === 0 && (
        <div className="flex flex-col items-center py-12 text-center">
          <AlertTriangle className="h-12 w-12 text-green-500" />
          <p className="mt-4 text-sm font-medium text-green-700">
            No hay multas pendientes. ¡Todo en orden!
          </p>
          <p className="text-sm text-muted-foreground">
            Las multas aparecerán aquí cuando se devuelvan libros con retraso.
          </p>
        </div>
      )}

      {/* Tabla */}
      {isLoading && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-muted-foreground">Cargando multas...</p>
        </div>
      )}

      {isError && (
        <div className="flex justify-center py-12">
          <p className="text-sm text-destructive">Ocurrió un error al cargar las multas.</p>
        </div>
      )}

      {multas && multas.length > 0 && (
        <div className="overflow-x-auto rounded-lg border">
          <table className="w-full text-sm">
            <thead className="bg-muted/50">
              <tr>
                <th className="px-4 py-3 text-left font-medium">Usuario</th>
                <th className="px-4 py-3 text-left font-medium">Libro</th>
                <th className="px-4 py-3 text-center font-medium">Días retraso</th>
                <th className="px-4 py-3 text-right font-medium">Monto total</th>
                <th className="px-4 py-3 text-left font-medium hidden md:table-cell">
                  Fecha generación
                </th>
                <th className="px-4 py-3 text-center font-medium">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y">
              {multas.map((multa) => (
                <tr key={multa.id} className="hover:bg-muted/30">
                  <td className="px-4 py-3 font-medium">{multa.nombreUsuario}</td>
                  <td className="px-4 py-3">{multa.tituloLibro}</td>
                  <td className="px-4 py-3 text-center">
                    <span className="font-medium text-red-600">{multa.diasRetraso} días</span>
                  </td>
                  <td className="px-4 py-3 text-right font-semibold">
                    {formatCOP(multa.montoTotal)}
                  </td>
                  <td className="px-4 py-3 hidden md:table-cell">
                    {formatDate(multa.fechaGeneracion)}
                  </td>
                  <td className="px-4 py-3 text-center">
                    <button
                      onClick={() => openPagoModal(multa)}
                      className="inline-flex items-center gap-1 rounded-md bg-green-600 px-3 py-1.5 text-xs font-medium text-white hover:bg-green-700"
                    >
                      <CreditCard className="h-3.5 w-3.5" />
                      Registrar pago
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Modal de pago */}
      {selectedMulta && (
        <RegistrarPagoModal
          multa={selectedMulta}
          open={pagoModalOpen}
          onClose={closePagoModal}
        />
      )}
    </div>
  );
}