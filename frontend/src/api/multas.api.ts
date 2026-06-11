import api from '@/lib/axios';
import type { MultaDto, MultasFiltros } from '@/types/multa.types';
import type { PagedResult } from '@/types/libro.types';

export async function getMultasPendientes(): Promise<MultaDto[]> {
  const response = await api.get<MultaDto[]>('/multas/pendientes');
  return response.data;
}

export async function getMultasByUsuario(
  usuarioId: string,
  filtros: MultasFiltros
): Promise<PagedResult<MultaDto>> {
  const params = new URLSearchParams();
  if (filtros.soloPendientes !== undefined) params.append('soloPendientes', String(filtros.soloPendientes));
  params.append('pagina', String(filtros.pagina));
  params.append('tamanoPagina', String(filtros.tamanoPagina));

  const response = await api.get<PagedResult<MultaDto>>(`/multas/usuario/${usuarioId}`, { params });
  return response.data;
}

export async function getMisMultas(
  pagina = 1,
  tamanoPagina = 10
): Promise<PagedResult<MultaDto>> {
  const response = await api.get<PagedResult<MultaDto>>('/multas/mis-multas', {
    params: { pagina, tamanoPagina },
  });
  return response.data;
}

export async function getMultaById(id: string): Promise<MultaDto> {
  const response = await api.get<MultaDto>(`/multas/${id}`);
  return response.data;
}

export async function registrarPago(id: string, observaciones?: string): Promise<MultaDto> {
  const response = await api.post<MultaDto>(`/multas/${id}/pagar`, {
    observaciones: observaciones ?? null,
  });
  return response.data;
}