using System;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string LastName { get; set; } = null!;
}
