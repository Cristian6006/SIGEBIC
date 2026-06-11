using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Multas.Queries;

public record GetMultaByIdQuery(Guid Id) : IRequest<MultaDto>;