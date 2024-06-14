using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models;

public partial class Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    public virtual User? UserNavigation { get; set; }
}
