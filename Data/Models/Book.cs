using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Data;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int PublisherId { get; set; }

    public int Year { get; set; }

    public int Pages { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Copy> Copies { get; set; } = new List<Copy>();

    public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<BookCategory> BookCategory { get; set; } = new List<BookCategory>();
}
