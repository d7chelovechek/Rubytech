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
        /// Инициализоровать маппер для должностей.
        /// </summary>
        /// <returns>Маппер данных для должностей.</returns>
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