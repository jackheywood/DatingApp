namespace DatingApp.Backend.Application.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string string1, string string2) =>
        string.Equals(string1, string2, StringComparison.CurrentCultureIgnoreCase);
}