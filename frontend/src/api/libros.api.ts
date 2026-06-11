import api from '@/lib/axios';
import type {
  CreateLibroRequest,
  LibroDto,
  LibrosFiltros,
  PagedResult,
  UpdateLibroRequest,
} from '@/types/libro.types';

export async function getLibros(
  filtros: LibrosFiltros
): Promise<PagedResult<LibroDto>> {
  const params = new URLSearchParams();

  if (filtros.titulo) params.append('titulo', filtros.titulo);
  if (filtros.autor) params.append('autor', filtros.autor);
  if (filtros.genero) params.append('genero', filtros.genero);
  if (filtros.soloDisponibles !== undefined)
    params.append('soloDisponibles', String(filtros.soloDisponibles));
  params.append('pagina', String(filtros.pagina));
  params.append('tamanoPagina', String(filtros.tamanoPagina));

  const response = await api.get<PagedResult<LibroDto>>('/libros', { params });
  return response.data;
}

export async function getLibroById(id: string): Promise<LibroDto> {
  const response = await api.get<LibroDto>(`/libros/${id}`);
  return response.data;
}

export async function createLibro(data: CreateLibroRequest): Promise<LibroDto> {
  const response = await api.post<LibroDto>('/libros', data);
  return response.data;
}

export async function updateLibro(
  id: string,
  data: UpdateLibroRequest
): Promise<LibroDto> {
  const response = await api.put<LibroDto>(`/libros/${id}`, data);
  return response.data;
}

export async function darDeBajaLibro(id: string): Promise<void> {
  await api.delete(`/libros/${id}`);
}