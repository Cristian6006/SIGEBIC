export interface LibroDto {
  id: string;
  isbn: string;
  titulo: string;
  autor: string;
  editorial: string | null;
  anoPublicacion: number;
  genero: string | null;
  cantidadTotal: number;
  cantidadDisponible: number;
  estado: string;
  estaDisponible: boolean;
}

export interface CreateLibroRequest {
  isbn: string;
  titulo: string;
  autor: string;
  editorial?: string;
  anoPublicacion: number;
  genero?: string;
  cantidadTotal: number;
}

export interface UpdateLibroRequest {
  isbn?: string;
  titulo?: string;
  autor?: string;
  editorial?: string;
  anoPublicacion?: number;
  genero?: string;
  cantidadTotal?: number;
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

export interface LibrosFiltros {
  titulo?: string;
  autor?: string;
  genero?: string;
  soloDisponibles?: boolean;
  pagina: number;
  tamanoPagina: number;
}