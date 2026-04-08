using AutoMapper;

namespace Booking.Application.Common.Interfaces;

public interface IAutoMap<TEntity, TDto> : IAutoMap
    where TEntity : class, new()
    where TDto : class, new()
{
    new void CreateMap(Profile profile)
    {
        profile.CreateMap<TEntity, TDto>()
            .ReverseMap();
    }
}

public interface IAutoMap
{
    void CreateMap(Profile profile)
    {
    }
}
