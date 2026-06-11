//import type { PagedResult } from './libro.types';

export interface PrestamoDto {
  id: string;
  usuarioId: string;
  nombreUsuario: string;
  libroId: string;
  tituloLibro: string;
  fechaPrestamo: string;
  fechaDevolucionEsperada: string;
  fechaDevolucionReal: string | null;
  estado: string;
  diasRestantes: number;
  estaVencido: boolean;
  cantidadRenovaciones: number;
  observaciones: string | null;
}

export interface HistorialPrestamoDto {
  id: string;
  prestamoId: string;
  libroId: string;
  tituloLibro: string;
  usuarioId: string;
  nombreUsuario: string;
  fechaPrestamo: string;
  fechaDevolucionReal: string;
  estadoFinal: string;
  diasRetraso: number;
  observaciones: string | null;
}

export interface RegistrarPrestamoRequest {
  usuarioId: string;
  libroId: string;
  diasPrestamo?: number;
  observaciones?: string;
}

export interface PrestamosFiltros {
  usuarioId?: string;
  libroId?: string;
  estado?: string;
  vencidos?: boolean;
  pagina: number;
  tamanoPagina: number;
}