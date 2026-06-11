import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  createLibro,
  darDeBajaLibro,
  getLibroById,
  getLibros,
  updateLibro,
} from '@/api/libros.api';
import type {
  CreateLibroRequest,
  LibrosFiltros,
  UpdateLibroRequest,
} from '@/types/libro.types';

export function useLibros(filtros: LibrosFiltros) {
  return useQuery({
    queryKey: ['libros', filtros],
    queryFn: () => getLibros(filtros),
  });
}

export function useLibroById(id: string | null) {
  return useQuery({
    queryKey: ['libros', id],
    queryFn: () => getLibroById(id!),
    enabled: !!id,
  });
}

export function useCreateLibro() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateLibroRequest) => createLibro(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['libros'] });
    },
  });
}

export function useUpdateLibro() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateLibroRequest }) =>
      updateLibro(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['libros'] });
    },
  });
}

export function useDarDeBajaLibro() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => darDeBajaLibro(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['libros'] });
    },
  });
}