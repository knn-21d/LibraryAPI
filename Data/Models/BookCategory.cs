using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Data;

[Table("book_category")]
public partial class BookCategory
{
    public string Isbn { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Book IsbnNavigation { get; set; } = null!;
}
