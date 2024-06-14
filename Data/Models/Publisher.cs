using System;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public partial class Publisher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
