using AutoMapper;
using Rubytech.Data.Models;
using Rubytech.Providers.Dtos;

namespace Rubytech.Providers.Mappers
{
    public static partial class MapperInitializator
    {
        public static Mapper InitializePositionMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<PositionDto, Position>();
            });

            return new Mapper(configuration);
        }
    }
}