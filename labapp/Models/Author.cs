using System.ComponentModel.DataAnnotations;

namespace labapp.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}

public class AuthorFormViewModel
{
    [Required(ErrorMessage = "Imię i nazwisko jest wymagane")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "2–100 znaków")]
    [Display(Name = "Imię i nazwisko")]
    public string Name { get; set; } = "";
}

public static class AuthorsMemory
{
    public static readonly List<Author> Authors = new()
    {
        new() { Id = 1, Name = "Stanisław Lem" },
        new() { Id = 2, Name = "Bolesław Prus" },
    };
}