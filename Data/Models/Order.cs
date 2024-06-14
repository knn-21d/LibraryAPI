using System;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int CopyId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Borrowed { get; set; }

    public DateTime? Returned { get; set; }

    public virtual Copy Copy { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
