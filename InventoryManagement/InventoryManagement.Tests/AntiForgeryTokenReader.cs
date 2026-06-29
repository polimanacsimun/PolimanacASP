using System.Text.RegularExpressions;

namespace InventoryManagement.Tests;

internal static partial class AntiForgeryTokenReader
{
    public static string Read(string html)
    {
        var match = TokenRegex().Match(html);

        if (!match.Success)
        {
            throw new InvalidOperationException("Antiforgery token was not found in the response HTML.");
        }

        return match.Groups["token"].Value;
    }

    [GeneratedRegex("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<token>[^\"]+)\"")]
    private static partial Regex TokenRegex();
}
