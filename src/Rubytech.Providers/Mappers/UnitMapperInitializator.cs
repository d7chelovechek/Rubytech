using AutoMapper;
using Rubytech.Data.Models;
using Rubytech.Providers.Dtos;

namespace Rubytech.Providers.Mappers
{
    public static partial class MapperInitializator
    {
        public static Mapper InitializeUnitMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<UnitDto, Unit>()
                    .ForMember(dest => dest.FullName, source => source.MapFrom(prop => prop.Name));
            });

            return new Mapper(configuration);
        }
    }
}