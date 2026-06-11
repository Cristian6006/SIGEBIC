using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Multas.Queries;

public record GetMultasPendientesQuery() : IRequest<IReadOnlyList<MultaDto>>;