import api from '@/lib/axios';
import type {
  CambiarPasswordRequest,
  CreateUsuarioRequest,
  PagedResult,
  RolDto,
  UpdateUsuarioRequest,
  UsuarioDto,
  UsuariosFiltros,
} from '@/types/usuario.types';

export async function getUsuarios(
  filtros: UsuariosFiltros
): Promise<PagedResult<UsuarioDto>> {
  const params = new URLSearchParams();

  if (filtros.nombre) params.append('nombre', filtros.nombre);
  if (filtros.email) params.append('email', filtros.email);
  if (filtros.rolId) params.append('rolId', filtros.rolId);
  if (filtros.activo !== undefined) params.append('activo', String(filtros.activo));
  params.append('pagina', String(filtros.pagina));
  params.append('tamanoPagina', String(filtros.tamanoPagina));

  const response = await api.get<PagedResult<UsuarioDto>>('/usuarios', { params });
  return response.data;
}

export async function getUsuarioById(id: string): Promise<UsuarioDto> {
  const response = await api.get<UsuarioDto>(`/usuarios/${id}`);
  return response.data;
}

export async function getPerfil(): Promise<UsuarioDto> {
  const response = await api.get<UsuarioDto>('/usuarios/perfil');
  return response.data;
}

export async function getRoles(): Promise<RolDto[]> {
  const response = await api.get<RolDto[]>('/roles');
  return response.data;
}

export async function createUsuario(
  data: CreateUsuarioRequest
): Promise<UsuarioDto> {
  const response = await api.post<UsuarioDto>('/usuarios', data);
  return response.data;
}

export async function updateUsuario(
  id: string,
  data: UpdateUsuarioRequest
): Promise<UsuarioDto> {
  const response = await api.put<UsuarioDto>(`/usuarios/${id}`, data);
  return response.data;
}

export async function cambiarPassword(
  id: string,
  data: CambiarPasswordRequest
): Promise<void> {
  await api.patch(`/usuarios/${id}/password`, data);
}

export async function toggleActivo(
  id: string,
  activar: boolean
): Promise<UsuarioDto> {
  const response = await api.patch<UsuarioDto>(`/usuarios/${id}/activo`, {
    activar,
  });
  return response.data;
}

export async function asignarRol(
  id: string,
  rolId: string
): Promise<UsuarioDto> {
  const response = await api.patch<UsuarioDto>(`/usuarios/${id}/rol`, {
    rolId,
  });
  return response.data;
}