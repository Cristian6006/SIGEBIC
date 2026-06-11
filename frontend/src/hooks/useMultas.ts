import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  getMultasPendientes,
  getMultasByUsuario,
  getMisMultas,
  getMultaById,
  registrarPago,
} from '@/api/multas.api';
import type { MultasFiltros } from '@/types/multa.types';

export function useMultasPendientes() {
  return useQuery({
    queryKey: ['multas', 'pendientes'],
    queryFn: getMultasPendientes,
    refetchInterval: 60_000,
  });
}

export function useMultasByUsuario(usuarioId: string, filtros: MultasFiltros) {
  return useQuery({
    queryKey: ['multas', 'usuario', usuarioId, filtros],
    queryFn: () => getMultasByUsuario(usuarioId, filtros),
    enabled: !!usuarioId,
  });
}

export function useMisMultas(pagina: number, tamanoPagina: number) {
  return useQuery({
    queryKey: ['multas', 'mias', pagina, tamanoPagina],
    queryFn: () => getMisMultas(pagina, tamanoPagina),
  });
}

export function useMultaById(id: string | null) {
  return useQuery({
    queryKey: ['multas', id],
    queryFn: () => getMultaById(id!),
    enabled: !!id,
  });
}

export function useRegistrarPago() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, observaciones }: { id: string; observaciones?: string }) =>
      registrarPago(id, observaciones),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['multas'] });
      queryClient.invalidateQueries({ queryKey: ['prestamos'] });
    },
  });
}