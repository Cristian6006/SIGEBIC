import { useState } from 'react';
import { useRegistrarPago } from '@/hooks/useMultas';
import type { MultaDto } from '@/types/multa.types';

interface RegistrarPagoModalProps {
  multa: MultaDto;
  open: boolean;
  onClose: () => void;
}

export default function RegistrarPagoModal({ multa, open, onClose }: RegistrarPagoModalProps) {
  const [observaciones, setObservaciones] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const registrarPago = useRegistrarPago();

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);

    try {
      await registrarPago.mutateAsync({
        id: multa.id,
        observaciones: observaciones || undefined,
      });
      setSuccess(true);
      setTimeout(() => handleClose(), 1500);
    } catch (err: any) {
      const serverError = err?.response?.data;
      if (serverError?.errors) {
        const messages = Object.values(serverError.errors).flat().join('. ');
        setError(messages);
      } else if (serverError?.title) {
        setError(serverError.title);
      } else {
        setError('Ocurrió un error al registrar el pago.');
      }
    }
  }

  function handleClose() {
    setObservaciones('');
    setError(null);
    setSuccess(false);
    onClose();
  }

  if (!open) return null;

  function formatCOP(amount: number) {
    return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', minimumFractionDigits: 0 }).format(amount);
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-md rounded-lg bg-white p-6 shadow-xl">
        <h2 className="text-lg font-semibold text-black">Registrar pago de multa</h2>

        {success ? (
          <div className="mt-4 rounded-md bg-green-50 p-4 text-sm text-green-800">
            Pago registrado exitosamente.
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="mt-4 space-y-4">
            {error && (
              <div className="rounded-md bg-red-50 p-3 text-sm text-red-800">{error}</div>
            )}

            <div className="rounded-md bg-muted/50 p-4 space-y-2">
              <div className="flex justify-between text-sm">
                <span className="text-muted-foreground">Usuario:</span>
                <span className="font-medium">{multa.nombreUsuario}</span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-muted-foreground">Libro:</span>
                <span className="font-medium">{multa.tituloLibro}</span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-muted-foreground">Días de retraso:</span>
                <span className="font-medium">{multa.diasRetraso} días</span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-muted-foreground">Monto total:</span>
                <span className="font-semibold text-red-600">{formatCOP(multa.montoTotal)}</span>
              </div>
            </div>

            <div>
              <label className="text-sm font-medium">Observaciones (opcional)</label>
              <textarea
                value={observaciones}
                onChange={(e) => setObservaciones(e.target.value)}
                rows={2}
                className="mt-1 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                placeholder="Notas sobre el pago..."
              />
            </div>

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
                disabled={registrarPago.isPending}
                className="rounded-md bg-green-600 px-4 py-2 text-sm font-medium text-white hover:bg-green-700 disabled:opacity-50"
              >
                {registrarPago.isPending ? 'Procesando...' : 'Confirmar pago'}
              </button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
}