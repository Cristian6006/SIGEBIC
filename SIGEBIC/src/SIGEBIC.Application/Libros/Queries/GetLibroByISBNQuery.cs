using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Libros.Queries;

public record GetLibroByISBNQuery(string ISBN) : IRequest<LibroDto>;