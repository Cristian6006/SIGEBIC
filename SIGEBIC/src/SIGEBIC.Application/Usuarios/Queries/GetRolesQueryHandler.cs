using System.Text.Json;
using MediatR;
using SIGEBIC.Application.DTOs;
using SIGEBIC.Domain.Interfaces;

namespace SIGEBIC.Application.Usuarios.Queries;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IReadOnlyList<RolDto>>
{
    private readonly IRolRepository _rolRepository;
    private readonly ICacheService _cacheService;

    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(1);

    public GetRolesQueryHandler(IRolRepository rolRepository, ICacheService cacheService)
    {
        _rolRepository = rolRepository;
        _cacheService = cacheService;
    }

    public async Task<IReadOnlyList<RolDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        // Intentar obtener desde Redis
        var cached = await _cacheService.GetAsync("roles:todos");
        if (cached is not null)
        {
            var deserialized = JsonSerializer.Deserialize<List<RolDto>>(cached);
            if (deserialized is not null)
                return deserialized;
        }

        var roles = await _rolRepository.GetAllAsync();
        var items = roles.Select(RolDto.FromEntity).ToList();

        // Guardar en Redis con TTL de 1 hora
        var serialized = JsonSerializer.Serialize(items);
        await _cacheService.SetAsync("roles:todos", serialized, CacheTtl);

        return items;
    }
}