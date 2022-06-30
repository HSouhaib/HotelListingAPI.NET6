namespace HotelListing.API.Core.CoreExceptions;

public class NotFoundException :ApplicationException
{
   public NotFoundException(string name, object key) :base($"{name} with id ({key}) was not found")
   {

   }

}
