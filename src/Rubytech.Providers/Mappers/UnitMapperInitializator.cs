using AutoMapper;
using Rubytech.Data.Models;
using Rubytech.Providers.Dtos;

namespace Rubytech.Providers.Mappers
{
    /// <summary>
    /// Инициализатор маппера данных.
    /// </summary>
    public static partial class MapperInitializator
    {
        /// <summary>
        /// Инициализоровать маппер для подразделений.
        /// </summary>
        /// <returns>Маппер данных для подразделений.</returns>
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