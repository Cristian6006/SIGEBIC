using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Libros.Queries;

public record GetLibroByIdQuery(Guid Id) : IRequest<LibroDto>;