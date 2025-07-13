using System.Text.Json;

namespace LoanProcessing.Application.Helpers;
public static class ObjectExtensions
{
    public static string AsJson(this object obj, bool indented = false)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = indented
        };

        return JsonSerializer.Serialize(obj, options);
    }
}

