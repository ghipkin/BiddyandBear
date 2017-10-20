using BB.DataLayer;
using BB.DataContracts;
using System.Diagnostics.CodeAnalysis;

namespace BB.Implementation
{
	[ExcludeFromCodeCoverage]
	public static class DLMapping
	{

		[ExcludeFromCodeCoverage]
		public static DL_BasketItem MapBasketItemtoDLBasketItem(BasketItem basketitem)
		{
			var result = new DL_BasketItem();
			result.CustomerId = basketitem.CustomerId;
			result.ItemId = basketitem.ItemId;
			result.Number = basketitem.Number;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public static DL_Customer MapCustomertoDLCustomer(Customer customer)
		{
			var result = new DL_Customer();
			result.Title = customer.Title;
			result.FirstName = customer.FirstName;
			result.LastName = customer.LastName;
			result.AddressLine1 = customer.AddressLine1;
			result.AddressLine2 = customer.AddressLine2;
			result.AddressLine3 = customer.AddressLine3;
			result.AddressLine4 = customer.AddressLine4;
			result.PostalCode = customer.PostalCode;
			result.Country = customer.Country;
			result.HomePhoneNo = customer.HomePhoneNo;
			result.MobilePhoneNo = customer.MobilePhoneNo;
			result.EmailAddress = customer.EmailAddress;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public static DL_Image MapImagetoDLImage(Image image)
		{
			var result = new DL_Image();
			result.ImageContent = image.ImageContent;
			result.ImageDescription = image.ImageDescription;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public static DL_Item MapItemtoDLItem(Item item)
		{
			var result = new DL_Item();
			result.Name = item.Name;
			result.Description = item.Description;
			result.Active = item.Active;
			result.Price = item.Price;
			result.Thumbnail = item.Thumbnail;
			result.Timestamp = item.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public static DL_Order MapOrdertoDLOrder(Order order)
		{
			var result = new DL_Order();
			result.DateOrderPlaced = order.DateOrderPlaced;
			result.DateOrderDispatched = order.DateOrderDispatched;
			result.SourceId = order.SourceId;
			result.Cancelled = order.Cancelled;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public static DL_Source MapSourcetoDLSource(Source source)
		{
			var result = new DL_Source();
			result.Description = source.Description;
			result.Name = source.Name;

			return result;
		}
	}
}
