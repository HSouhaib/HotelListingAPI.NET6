namespace HotelListing.API.Core.CoreModels;

public class PageResult<T>
{
   public int TotalCount { get; set; }
   public int PageNumber { get; set; }
   public int RecordNumber { get; set; }
   public List<T> items { get; set; }

}
