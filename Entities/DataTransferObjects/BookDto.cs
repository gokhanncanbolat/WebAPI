using System.Text.Json.Serialization;

namespace Entities.DataTransferObjects
{
    //[JsonSerializable(typeof(BookDto))]

    //public record BookDto(int id, string Title, decimal Price);

    // response`nin daha duzenli geldigi format asagidaki gibidir. Serializa`ye gerek kalmadi

    public record BookDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public decimal Price { get; init; }
    }

}
