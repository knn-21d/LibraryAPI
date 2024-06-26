﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Data;

public partial class AuthorBook
{
    public int AuthorId { get; set; }

    public string Isbn { get; set; } = null!;

    public virtual Author Author { get; set; } = null!;

    public virtual Book IsbnNavigation { get; set; } = null!;
}
