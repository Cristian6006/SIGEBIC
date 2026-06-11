using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Enums;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.Queries;

public class GetPrestamosQueryHandler : IRequestHandler<GetPrestamosQuery, PagedResult<PrestamoDto>>
{
    private readonly IPrestamoRepository _prestamoRepository;

    public GetPrestamosQueryHandler(IPrestamoRepository prestamoRepository)
    {
        _prestamoRepository = prestamoRepository;
    }

    public async Task<PagedResult<PrestamoDto>> Handle(GetPrestamosQuery request, CancellationToken cancellationToken)
    {
        var spec = new PrestamoQuerySpecification(
            request.UsuarioId,
            request.LibroId,
            request.Estado,
            request.Vencidos,
            request.Pagina,
            request.TamanoPagina);

        var prestamos = await _prestamoRepository.GetAllAsync(spec);
        var totalRegistros = await _prestamoRepository.GetCountAsync(spec);

        var items = prestamos
            .Select(PrestamoDto.FromEntity)
            .ToList();

        return new PagedResult<PrestamoDto>(
            items,
            request.Pagina,
            request.TamanoPagina,
            totalRegistros);
    }

    private sealed class PrestamoQuerySpecification : IPrestamoSpecification
    {
        public Guid? UsuarioId { get; }
        public Guid? LibroId { get; }
        public EstadoPrestamo? Estado { get; }
        public bool? Vencidos { get; }
        public int Pagina { get; }
        public int TamanoPagina { get; }

        public PrestamoQuerySpecification(
            Guid? usuarioId,
            Guid? libroId,
            EstadoPrestamo? estado,
            bool? vencidos,
            int pagina,
            int tamanoPagina)
        {
            UsuarioId = usuarioId;
            LibroId = libroId;
            Estado = estado;
            Vencidos = vencidos;
            Pagina = pagina;
            TamanoPagina = tamanoPagina;
        }
    }
}