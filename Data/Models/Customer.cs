using System;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? AltPhone { get; set; }

    public string Address { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
