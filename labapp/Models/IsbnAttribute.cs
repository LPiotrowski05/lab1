using System.ComponentModel.DataAnnotations;

namespace labapp.Models;

public class IsbnAttribute : ValidationAttribute
{
    public bool RequireChecksum { get; set; } = false;

    public IsbnAttribute() => ErrorMessage = "Niepoprawny numer ISBN (10 lub 13 cyfr)";

    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        if (value is not string s) return false;
        if (string.IsNullOrWhiteSpace(s)) return true;

        var clean = s.Replace("-", "").Replace(" ", "").ToUpperInvariant();

        if (clean.Length == 13 && clean.All(char.IsDigit))
            return !RequireChecksum || Isbn13Ok(clean);

        if (clean.Length == 10 && clean[..9].All(char.IsDigit) &&
            (char.IsDigit(clean[9]) || clean[9] == 'X'))
            return !RequireChecksum || Isbn10Ok(clean);

        return false;
    }

    private static bool Isbn13Ok(string d)
    {
        int sum = 0;
        for (int i = 0; i < 13; i++)
            sum += (d[i] - '0') * (i % 2 == 0 ? 1 : 3);
        return sum % 10 == 0;
    }

    private static bool Isbn10Ok(string d)
    {
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            int v = d[i] == 'X' ? 10 : d[i] - '0';
            sum += v * (10 - i);
        }
        return sum % 11 == 0;
    }
}
