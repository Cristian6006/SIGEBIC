using MediatR;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Auth.Commands;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly ITokenService _tokenService;

    public LogoutCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Revocar el token (el token vendrá en el header Authorization y se extraerá en el controller)
        await _tokenService.RevocarToken(request.Token);
    }
}
