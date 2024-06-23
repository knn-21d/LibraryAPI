namespace LibraryAPI.Data.DTOs
{
    public class BookDto
    {
        public string Isbn { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int? PublisherId { get; set; }

        public int Year { get; set; }

        public int Pages { get; set; }

        public decimal Price { get; set; }

        public int[]? AuthorId { get; set; } = null;

        public virtual PublisherDto? Publisher { get; set; } = null!;

        public virtual AuthorDto[]? Author { get; set; } = null;
    }

}
