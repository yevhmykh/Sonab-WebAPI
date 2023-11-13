namespace Sonab.Core.Constants;

public static class Limits
{
    /// <summary>
    /// Entity name minimal length
    /// </summary>
    public const int NameMinLength = 2;
    /// <summary>
    /// Article title minimal length
    /// </summary>
    public const int TitleMinLength = 2;

    /// <summary>
    /// Article title maximum length
    /// </summary>
    public const int TitleMaxLength = 100;

    /// <summary>
    /// Article content minimal length
    /// </summary>
    public const int ContentMinLength = 200;

    /// <summary>
    /// Max topic tag count which server return in response
    /// </summary>
    public const int TopicCount = 20;
}
