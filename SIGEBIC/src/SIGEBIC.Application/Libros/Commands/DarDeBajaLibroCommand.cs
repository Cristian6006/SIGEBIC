using MediatR;

namespace SIGEBIC.Application.Libros.Commands;

public record DarDeBajaLibroCommand(Guid Id) : IRequest;