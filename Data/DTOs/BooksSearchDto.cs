namespace LibraryAPI.Data.DTOs
{
    public class BooksSearchDto
    {
        public string? Name { get; set; } = null;

        public string? Country { get; set; } = null;

        public int? Year { get; set; } = null;

        public int? PagesFrom { get; set; } = null;

        public int? PagesTo { get; set; } = null;

        public decimal? PriceFrom { get; set; } = null;

        public decimal? PriceTo { get; set; } = null;

        public string? Author { get; set; } = null;

        public string? PublisherName { get; set; } = null;


    }
}
