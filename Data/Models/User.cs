using System;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Role Role { get; set; } = null!;
}
