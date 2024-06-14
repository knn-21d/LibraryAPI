using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models;

public partial class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int CopyId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Borrowed { get; set; }

    public DateTime? Returned { get; set; }

    public virtual Copy Copy { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;
}
