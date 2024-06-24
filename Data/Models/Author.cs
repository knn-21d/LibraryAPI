using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Data;

public partial class Author
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string LastName { get; set; } = null!;

    public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
}
