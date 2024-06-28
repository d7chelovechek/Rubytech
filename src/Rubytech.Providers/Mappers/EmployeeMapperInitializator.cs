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
        /// Инициализоровать маппер для сотрудников.
        /// </summary>
        /// <returns>Маппер данных для сотрудников.</returns>
        public static Mapper InitializeEmployeeMapper()
        {
            var configuration = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<EmployeeDto, Employee>()
                    .ForMember(dest => dest.IsMainJob, source => source.MapFrom(prop => prop.IsMain))
                    .ForMember(dest => dest.StartDate, source => source.MapFrom(prop => prop.DateFrom));
            });

            return new Mapper(configuration);
        }
    }
}