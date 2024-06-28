namespace Rubytech.Lib.Exceptions
{
    /// <summary>
    /// Исключение несконфигурированного архиватора данных.
    /// </summary>
    public class DataArchiverNotConfiguredException() : Exception("Архиватор данных не был сконфигурирован.");
}