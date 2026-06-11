using MediatR;

namespace SIGEBIC.Application.Auth.Commands;

public record LogoutCommand(Guid UserId, string Token) : IRequest;
