using AutoMapper;
using Itbeard.Mappings;

namespace Itbeard.Shortener.Common;

public static class MappersExtensions
{
    public static void AddCustomAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}