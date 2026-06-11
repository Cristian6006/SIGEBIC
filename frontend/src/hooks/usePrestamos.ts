import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  getHistorialByLibro,
  getMiHistorial,
  getMisPrestamos,
  getPrestamoById,
  getPrestamos,
  getPrestamosVencidos,
  registrarDevolucion,
  registrarPrestamo,
  renovarPrestamo,
} from '@/api/prestamos.api';
import type {
  PrestamosFiltros,
  RegistrarPrestamoRequest,
} from '@/types/prestamo.types';

export function usePrestamos(filtros: PrestamosFiltros) {
  return useQuery({
    queryKey: ['prestamos', filtros],
    queryFn: () => getPrestamos(filtros),
  });
}

export function usePrestamosVencidos() {
  return useQuery({
    queryKey: ['prestamos', 'vencidos'],
    queryFn: getPrestamosVencidos,
  });
}

export function useMisPrestamos(pagina: number, tamanoPagina: number) {
  return useQuery({
    queryKey: ['prestamos', 'mis-prestamos', pagina, tamanoPagina],
    queryFn: () => getMisPrestamos(pagina, tamanoPagina),
  });
}

export function usePrestamoById(id: string | null) {
  return useQuery({
    queryKey: ['prestamos', id],
    queryFn: () => getPrestamoById(id!),
    enabled: !!id,
  });
}

export function useRegistrarPrestamo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: RegistrarPrestamoRequest) => registrarPrestamo(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['prestamos'] });
      queryClient.invalidateQueries({ queryKey: ['libros'] });
    },
  });
}

export function useRegistrarDevolucion() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, observaciones }: { id: string; observaciones?: string }) =>
      registrarDevolucion(id, observaciones),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['prestamos'] });
      queryClient.invalidateQueries({ queryKey: ['libros'] });
    },
  });
}

export function useRenovarPrestamo() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, diasExtension }: { id: string; diasExtension?: number }) =>
      renovarPrestamo(id, diasExtension),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['prestamos'] });
    },
  });
}

export function useHistorialByLibro(libroId: string | null, pagina: number) {
  return useQuery({
    queryKey: ['historial', 'libro', libroId, pagina],
    queryFn: () => getHistorialByLibro(libroId!, pagina, 10),
    enabled: !!libroId,
  });
}

export function useMiHistorial(pagina: number) {
  return useQuery({
    queryKey: ['historial', 'mi-historial', pagina],
    queryFn: () => getMiHistorial(pagina, 10),
  });
}