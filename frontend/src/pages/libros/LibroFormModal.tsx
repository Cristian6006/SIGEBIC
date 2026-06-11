import { useState, useEffect } from 'react';
import { X } from 'lucide-react';
import { useCreateLibro, useUpdateLibro } from '@/hooks/useLibros';
import type { LibroDto, CreateLibroRequest, UpdateLibroRequest } from '@/types/libro.types';
import type { AxiosError } from 'axios';

interface LibroFormModalProps {
  open: boolean;
  onClose: () => void;
  libro?: LibroDto | null;
}

interface BackendValidationErrors {
  errors?: Record<string, string[]>;
}

export default function LibroFormModal({ open, onClose, libro }: LibroFormModalProps) {
  const [isbn, setIsbn] = useState('');
  const [titulo, setTitulo] = useState('');
  const [autor, setAutor] = useState('');
  const [editorial, setEditorial] = useState('');
  const [anoPublicacion, setAnoPublicacion] = useState('');
  const [genero, setGenero] = useState('');
  const [cantidadTotal, setCantidadTotal] = useState('');

  const [fieldErrors, setFieldErrors] = useState<Record<string, string>>({});

  const createLibro = useCreateLibro();
  const updateLibro = useUpdateLibro();

  const isEditing = !!libro;

  useEffect(() => {
    if (libro) {
      setIsbn(libro.isbn);
      setTitulo(libro.titulo);
      setAutor(libro.autor);
      setEditorial(libro.editorial ?? '');
      setAnoPublicacion(String(libro.anoPublicacion));
      setGenero(libro.genero ?? '');
      setCantidadTotal(String(libro.cantidadTotal));
    } else {
      setIsbn('');
      setTitulo('');
      setAutor('');
      setEditorial('');
      setAnoPublicacion('');
      setGenero('');
      setCantidadTotal('');
    }
    setFieldErrors({});
  }, [libro, open]);

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

    const datos: CreateLibroRequest = {
      isbn: isbn.trim(),
      titulo: titulo.trim(),
      autor: autor.trim(),
      editorial: editorial.trim() || undefined,
      anoPublicacion: parseInt(anoPublicacion, 10),
      genero: genero.trim() || undefined,
      cantidadTotal: parseInt(cantidadTotal, 10),
    };

    try {
      if (isEditing) {
        const updateDatos: UpdateLibroRequest = {
          isbn: isbn.trim() || undefined,
          titulo: titulo.trim() || undefined,
          autor: autor.trim() || undefined,
          editorial: editorial.trim() || undefined,
          anoPublicacion: anoPublicacion ? parseInt(anoPublicacion, 10) : undefined,
          genero: genero.trim() || undefined,
          cantidadTotal: cantidadTotal ? parseInt(cantidadTotal, 10) : undefined,
        };
        await updateLibro.mutateAsync({ id: libro.id, data: updateDatos });
      } else {
        await createLibro.mutateAsync(datos);
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
            {isEditing ? 'Editar libro' : 'Nuevo libro'}
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
            <label className="text-sm font-medium">ISBN</label>
            <input
              type="text"
              value={isbn}
              onChange={(e) => setIsbn(e.target.value)}
              maxLength={20}
              required
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['ISBN'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['ISBN']}</p>
            )}
          </div>

          <div>
            <label className="text-sm font-medium">Título</label>
            <input
              type="text"
              value={titulo}
              onChange={(e) => setTitulo(e.target.value)}
              maxLength={200}
              required
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['Titulo'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['Titulo']}</p>
            )}
          </div>

          <div>
            <label className="text-sm font-medium">Autor</label>
            <input
              type="text"
              value={autor}
              onChange={(e) => setAutor(e.target.value)}
              maxLength={150}
              required
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
            {fieldErrors['Autor'] && (
              <p className="mt-1 text-xs text-destructive">{fieldErrors['Autor']}</p>
            )}
          </div>

          <div>
            <label className="text-sm font-medium">Editorial</label>
            <input
              type="text"
              value={editorial}
              onChange={(e) => setEditorial(e.target.value)}
              maxLength={150}
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="text-sm font-medium">Año de publicación</label>
              <input
                type="number"
                value={anoPublicacion}
                onChange={(e) => setAnoPublicacion(e.target.value)}
                min={1000}
                max={new Date().getFullYear()}
                required
                className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
              />
              {fieldErrors['AnoPublicacion'] && (
                <p className="mt-1 text-xs text-destructive">{fieldErrors['AnoPublicacion']}</p>
              )}
            </div>

            <div>
              <label className="text-sm font-medium">Cantidad total</label>
              <input
                type="number"
                value={cantidadTotal}
                onChange={(e) => setCantidadTotal(e.target.value)}
                min={1}
                required
                className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
              />
              {fieldErrors['CantidadTotal'] && (
                <p className="mt-1 text-xs text-destructive">{fieldErrors['CantidadTotal']}</p>
              )}
            </div>
          </div>

          <div>
            <label className="text-sm font-medium">Género</label>
            <input
              type="text"
              value={genero}
              onChange={(e) => setGenero(e.target.value)}
              maxLength={80}
              className="w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            />
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
              disabled={createLibro.isPending || updateLibro.isPending}
              className="rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:bg-primary/90 disabled:opacity-50"
            >
              {createLibro.isPending || updateLibro.isPending
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