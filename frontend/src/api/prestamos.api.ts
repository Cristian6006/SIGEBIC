import api from '@/lib/axios';
import type {
  HistorialPrestamoDto,
  PrestamoDto,
  PrestamosFiltros,
  RegistrarPrestamoRequest,
} from '@/types/prestamo.types';
import type { PagedResult } from '@/types/libro.types';

export async function getPrestamos(
  filtros: PrestamosFiltros
): Promise<PagedResult<PrestamoDto>> {
  const params = new URLSearchParams();
  if (filtros.usuarioId) params.append('usuarioId', filtros.usuarioId);
  if (filtros.libroId) params.append('libroId', filtros.libroId);
  if (filtros.estado) params.append('estado', filtros.estado);
  if (filtros.vencidos !== undefined) params.append('vencidos', String(filtros.vencidos));
  params.append('pagina', String(filtros.pagina));
  params.append('tamanoPagina', String(filtros.tamanoPagina));

  const response = await api.get<PagedResult<PrestamoDto>>('/prestamos', { params });
  return response.data;
}

export async function getPrestamosVencidos(): Promise<PrestamoDto[]> {
  const response = await api.get<PrestamoDto[]>('/prestamos/vencidos');
  return response.data;
}

export async function getMisPrestamos(
  pagina = 1,
  tamanoPagina = 10
): Promise<PagedResult<PrestamoDto>> {
  const response = await api.get<PagedResult<PrestamoDto>>('/prestamos/mis-prestamos', {
    params: { pagina, tamanoPagina },
  });
  return response.data;
}

export async function getPrestamoById(id: string): Promise<PrestamoDto> {
  const response = await api.get<PrestamoDto>(`/prestamos/${id}`);
  return response.data;
}

export async function registrarPrestamo(
  data: RegistrarPrestamoRequest
): Promise<PrestamoDto> {
  const response = await api.post<PrestamoDto>('/prestamos', data);
  return response.data;
}

export async function registrarDevolucion(
  id: string,
  observaciones?: string
): Promise<PrestamoDto> {
  const response = await api.post<PrestamoDto>(`/prestamos/${id}/devolver`, {
    observaciones: observaciones ?? null,
  });
  return response.data;
}

export async function renovarPrestamo(
  id: string,
  diasExtension = 7
): Promise<PrestamoDto> {
  const response = await api.post<PrestamoDto>(`/prestamos/${id}/renovar`, {
    diasExtension,
  });
  return response.data;
}

export async function getHistorialByLibro(
  libroId: string,
  pagina = 1,
  tamanoPagina = 10
): Promise<PagedResult<HistorialPrestamoDto>> {
  const response = await api.get<PagedResult<HistorialPrestamoDto>>(
    `/historial/libros/${libroId}`,
    { params: { pagina, tamanoPagina } }
  );
  return response.data;
}

export async function getMiHistorial(
  pagina = 1,
  tamanoPagina = 10
): Promise<PagedResult<HistorialPrestamoDto>> {
  const response = await api.get<PagedResult<HistorialPrestamoDto>>(
    '/historial/mi-historial',
    { params: { pagina, tamanoPagina } }
  );
  return response.data;
}