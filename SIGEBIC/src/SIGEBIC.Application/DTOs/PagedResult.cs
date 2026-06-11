namespace SIGEBIC.Application.DTOs;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int PaginaActual,
    int TamanoPagina,
    int TotalRegistros)
{
    public int TotalPaginas => (int)Math.Ceiling(TotalRegistros / (double)TamanoPagina);
    public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    public bool TienePaginaAnterior => PaginaActual > 1;
}