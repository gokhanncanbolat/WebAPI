namespace Entities.Exceptions
{
    public class PriceOutOfRangeBadRequest : BadRequestException
    {
        public PriceOutOfRangeBadRequest() : base("Maximum price should be less than 1000 and greater than 10.")
        {

        }
    }
}
