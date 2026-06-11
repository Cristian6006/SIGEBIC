namespace SIGEBIC.Domain.Interfaces;

public interface ITokenService
{
    string GenerarToken(Domain.Entities.Usuario usuario);
    Task RevocarToken(string token);
}