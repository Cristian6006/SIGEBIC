export interface UsuarioDto {
  id: string;
  nombre: string;
  apellido: string;
  email: string;
  telefono: string;
  numeroDocumento: string;
  fechaRegistro: string;
  activo: boolean;
  rolId: string;
  nombreRol: string;
}

export interface RolDto {
  id: string;
  nombre: string;
  descripcion: string;
}

export interface CreateUsuarioRequest {
  nombre: string;
  apellido: string;
  email: string;
  password: string;
  telefono?: string;
  numeroDocumento: string;
  rolId: string;
}

export interface UpdateUsuarioRequest {
  nombre?: string;
  apellido?: string;
  telefono?: string;
  numeroDocumento?: string;
}

export interface CambiarPasswordRequest {
  passwordActual: string;
  nuevoPassword: string;
}

export interface UsuariosFiltros {
  nombre?: string;
  email?: string;
  rolId?: string;
  activo?: boolean;
  pagina: number;
  tamanoPagina: number;
}

export interface PagedResult<T> {
  items: T[];
  paginaActual: number;
  tamanoPagina: number;
  totalRegistros: number;
  totalPaginas: number;
  tienePaginaSiguiente: boolean;
  tienePaginaAnterior: boolean;
}