using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Enums;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Prestamos.Queries;

public class GetMisPrestamosQueryHandler : IRequestHandler<GetMisPrestamosQuery, PagedResult<PrestamoDto>>
{
    private readonly IPrestamoRepository _prestamoRepository;

    public GetMisPrestamosQueryHandler(IPrestamoRepository prestamoRepository)
    {
        _prestamoRepository = prestamoRepository;
    }

    public async Task<PagedResult<PrestamoDto>> Handle(GetMisPrestamosQuery request, CancellationToken cancellationToken)
    {
        var spec = new MisPrestamosSpecification(
            request.UsuarioId,
            request.Estado,
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

    private sealed class MisPrestamosSpecification : IPrestamoSpecification
    {
        public Guid? UsuarioId { get; }
        public Guid? LibroId => null;
        public EstadoPrestamo? Estado { get; }
        public bool? Vencidos => null;
        public int Pagina { get; }
        public int TamanoPagina { get; }

        public MisPrestamosSpecification(
            Guid usuarioId,
            EstadoPrestamo? estado,
            int pagina,
            int tamanoPagina)
        {
            UsuarioId = usuarioId;
            Estado = estado;
            Pagina = pagina;
            TamanoPagina = tamanoPagina;
        }
    }
}