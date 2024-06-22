using System.Text.Json.Serialization;

namespace LibraryAPI.Data.DTOs;

    public class NewOrderDto
    {
    public string isbn { get; set; }

    [JsonConstructor]
    public NewOrderDto(string isbn)
    {
        this.isbn = isbn;
    }
}

