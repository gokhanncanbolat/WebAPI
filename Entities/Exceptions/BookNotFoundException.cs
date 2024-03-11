namespace Entities.Exceptions
{
    // sealed kalitima kapali demek
    public sealed class BookNotFoundException : NotFoundException
    {
        public BookNotFoundException(int id) : base($"The book with id : {id} could not found.")
        {

        }
    }
}
