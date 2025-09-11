using System.Text;
using System.Text.RegularExpressions;

namespace PharmaCosmetics.Web.Services;
public static class SlugHelper
{
    public static string Generate(string phrase)
    {
        var str = phrase.ToLowerInvariant().Trim();
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = str[..(str.Length > 80 ? 80 : str.Length)];
        str = Regex.Replace(str, @"\s", "-");
        return str;
    }
}