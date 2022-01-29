/// <summary>
/// I like keeping the Command and the CommandHandler together 
/// as it provides a better visibility as well as quick access.
/// </summary>
namespace TvMazeScraper.Application.Extensions;

public static class DateTimeExtensions
{
    public static string TryFormatDate(this DateTime? me, string format)
    {
        return
            me.HasValue
            ? me.Value.ToString(format)
            : string.Empty;
    }
}