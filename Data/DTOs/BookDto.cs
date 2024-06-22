﻿namespace LibraryAPI.Data.DTOs
{
    public class BookDto
    {
        public string Isbn { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int? PublisherId { get; set; }

        public int Year { get; set; }

        public int Pages { get; set; }

        public decimal Price { get; set; }

        public virtual PublisherDto? Publisher { get; set; } = null!;
    }

}
