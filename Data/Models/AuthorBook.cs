using System;
using System.Collections.Generic;

namespace LibraryAPI.Models;

public partial class AuthorBook
{
    public int AuthorId { get; set; }

    public string Isbn { get; set; } = null!;

    public virtual Author Author { get; set; } = null!;

    public virtual Book IsbnNavigation { get; set; } = null!;
}
