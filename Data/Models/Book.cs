using System;
using System.Collections.Generic;

namespace LibraryAPI.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int PublisherId { get; set; }

    public int Year { get; set; }

    public int Pages { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Copy> Copies { get; set; } = new List<Copy>();

    public virtual Publisher Publisher { get; set; } = null!;
}
