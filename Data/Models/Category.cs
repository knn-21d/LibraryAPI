﻿using System;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookCategory> BookCategory { get; set; } = new List<BookCategory>();

}
