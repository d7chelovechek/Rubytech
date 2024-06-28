namespace Rubytech.Lib.Exceptions
{
    /// <summary>
    /// Исключение несконфигурированного поставщика данных.
    /// </summary>
    public class DataProviderNotConfiguredException() : Exception("Провайдер данных не был сконфигурирован.");
}