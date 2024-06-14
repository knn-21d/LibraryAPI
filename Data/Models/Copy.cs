using System;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public partial class Copy
{
    public int Id { get; set; }

    public string Isbn { get; set; } = null!;

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
