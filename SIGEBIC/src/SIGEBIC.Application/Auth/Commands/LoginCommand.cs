using MediatR;
using SIGEBIC.Application.DTOs;

namespace SIGEBIC.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
