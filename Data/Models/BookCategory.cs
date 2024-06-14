using System;
using System.Collections.Generic;

namespace LibraryAPI.Models;

public partial class BookCategory
{
    public string Isbn { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Book IsbnNavigation { get; set; } = null!;
}
