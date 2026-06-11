export interface MultaDto {
  id: string;
  prestamoId: string;
  tituloLibro: string;
  usuarioId: string;
  nombreUsuario: string;
  montoPorDia: number;
  diasRetraso: number;
  montoTotal: number;
  pagada: boolean;
  fechaPago: string | null;
  fechaGeneracion: string;
  observaciones: string | null;
}

export interface MultasFiltros {
  soloPendientes?: boolean;
  pagina: number;
  tamanoPagina: number;
}