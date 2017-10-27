using BB.DataLayer;
using BB.DataContracts;
using System.Diagnostics.CodeAnalysis;

namespace BB.Implementation
{
	[ExcludeFromCodeCoverage]
	public abstract class ADL_Mapping
	{

		[ExcludeFromCodeCoverage]
		public abstract BasketItem MapBasketItemfromDLBasketItem(ADL_BasketItem basketitem);


		[ExcludeFromCodeCoverage]
		public abstract ADL_BasketItem MapBasketItemtoDLBasketItem(BasketItem basketitem);


		[ExcludeFromCodeCoverage]
		public abstract CurrentItem MapCurrentItemfromDLCurrentItem(ADL_CurrentItem currentitem);


		[ExcludeFromCodeCoverage]
		public abstract ADL_CurrentItem MapCurrentItemtoDLCurrentItem(CurrentItem currentitem);


		[ExcludeFromCodeCoverage]
		public abstract CurrentItemCategory MapCurrentItemCategoryfromDLCurrentItemCategory(ADL_CurrentItemCategory currentitemcategory);


		[ExcludeFromCodeCoverage]
		public abstract ADL_CurrentItemCategory MapCurrentItemCategorytoDLCurrentItemCategory(CurrentItemCategory currentitemcategory);


		[ExcludeFromCodeCoverage]
		public abstract Customer MapCustomerfromDLCustomer(ADL_Customer customer);


		[ExcludeFromCodeCoverage]
		public abstract ADL_Customer MapCustomertoDLCustomer(Customer customer);


		[ExcludeFromCodeCoverage]
		public abstract Image MapImagefromDLImage(ADL_Image image);


		[ExcludeFromCodeCoverage]
		public abstract ADL_Image MapImagetoDLImage(Image image);


		[ExcludeFromCodeCoverage]
		public abstract Item MapItemfromDLItem(ADL_Item item);


		[ExcludeFromCodeCoverage]
		public abstract ADL_Item MapItemtoDLItem(Item item);


		[ExcludeFromCodeCoverage]
		public abstract ItemCategory MapItemCategoryfromDLItemCategory(ADL_ItemCategory itemcategory);


		[ExcludeFromCodeCoverage]
		public abstract ADL_ItemCategory MapItemCategorytoDLItemCategory(ItemCategory itemcategory);


		[ExcludeFromCodeCoverage]
		public abstract ItemImages MapItemImagesfromDLItemImages(ADL_ItemImages itemimages);


		[ExcludeFromCodeCoverage]
		public abstract ADL_ItemImages MapItemImagestoDLItemImages(ItemImages itemimages);


		[ExcludeFromCodeCoverage]
		public abstract Order MapOrderfromDLOrder(ADL_Order order);


		[ExcludeFromCodeCoverage]
		public abstract ADL_Order MapOrdertoDLOrder(Order order);


		[ExcludeFromCodeCoverage]
		public abstract OrderLine MapOrderLinefromDLOrderLine(ADL_OrderLine orderline);


		[ExcludeFromCodeCoverage]
		public abstract ADL_OrderLine MapOrderLinetoDLOrderLine(OrderLine orderline);


		[ExcludeFromCodeCoverage]
		public abstract PreviousPassword MapPreviousPasswordfromDLPreviousPassword(ADL_PreviousPassword previouspassword);


		[ExcludeFromCodeCoverage]
		public abstract ADL_PreviousPassword MapPreviousPasswordtoDLPreviousPassword(PreviousPassword previouspassword);


		[ExcludeFromCodeCoverage]
		public abstract Source MapSourcefromDLSource(ADL_Source source);


		[ExcludeFromCodeCoverage]
		public abstract ADL_Source MapSourcetoDLSource(Source source);

	}

	[ExcludeFromCodeCoverage]
	public class DLMapping : ADL_Mapping	{

		[ExcludeFromCodeCoverage]
		public override BasketItem MapBasketItemfromDLBasketItem(ADL_BasketItem basketitem)
		{
			var result = new BasketItem();
			result.CustomerId = basketitem.CustomerId;
			result.ItemId = basketitem.ItemId;
			result.Number = basketitem.Number;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_BasketItem MapBasketItemtoDLBasketItem(BasketItem basketitem)
		{
			var result = new DL_BasketItem();
			result.CustomerId = basketitem.CustomerId;
			result.ItemId = basketitem.ItemId;
			result.Number = basketitem.Number;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override CurrentItem MapCurrentItemfromDLCurrentItem(ADL_CurrentItem currentitem)
		{
			var result = new CurrentItem();
			result.Id = currentitem.Id;
			result.Name = currentitem.Name;
			result.Description = currentitem.Description;
			result.CategoryId = currentitem.CategoryId;
			result.Price = currentitem.Price;
			result.Thumbnail = currentitem.Thumbnail;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_CurrentItem MapCurrentItemtoDLCurrentItem(CurrentItem currentitem)
		{
			var result = new DL_CurrentItem();
			result.Name = currentitem.Name;
			result.Description = currentitem.Description;
			result.CategoryId = currentitem.CategoryId;
			result.Price = currentitem.Price;
			result.Thumbnail = currentitem.Thumbnail;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override CurrentItemCategory MapCurrentItemCategoryfromDLCurrentItemCategory(ADL_CurrentItemCategory currentitemcategory)
		{
			var result = new CurrentItemCategory();
			result.Id = currentitemcategory.Id;
			result.Name = currentitemcategory.Name;
			result.Description = currentitemcategory.Description;
			result.VAT = currentitemcategory.VAT;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_CurrentItemCategory MapCurrentItemCategorytoDLCurrentItemCategory(CurrentItemCategory currentitemcategory)
		{
			var result = new DL_CurrentItemCategory();
			result.Name = currentitemcategory.Name;
			result.Description = currentitemcategory.Description;
			result.VAT = currentitemcategory.VAT;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Customer MapCustomerfromDLCustomer(ADL_Customer customer)
		{
			var result = new Customer();
			result.Id = customer.Id;
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
			result.UserName = customer.UserName;
			result.Salt = customer.Salt;
			result.PasswordHash = customer.PasswordHash;
			result.PasswordNeedsChanging = customer.PasswordNeedsChanging;
			result.Timestamp = customer.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Customer MapCustomertoDLCustomer(Customer customer)
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
			result.UserName = customer.UserName;
			result.Salt = customer.Salt;
			result.PasswordHash = customer.PasswordHash;
			result.PasswordNeedsChanging = customer.PasswordNeedsChanging;
			result.Timestamp = customer.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Image MapImagefromDLImage(ADL_Image image)
		{
			var result = new Image();
			result.Id = image.Id;
			result.ImageContent = image.ImageContent;
			result.ImageDescription = image.ImageDescription;
			result.Timestamp = image.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Image MapImagetoDLImage(Image image)
		{
			var result = new DL_Image();
			result.ImageContent = image.ImageContent;
			result.ImageDescription = image.ImageDescription;
			result.Timestamp = image.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Item MapItemfromDLItem(ADL_Item item)
		{
			var result = new Item();
			result.Id = item.Id;
			result.Name = item.Name;
			result.Description = item.Description;
			result.CategoryId = item.CategoryId;
			result.Active = item.Active;
			result.Price = item.Price;
			result.Thumbnail = item.Thumbnail;
			result.Timestamp = item.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Item MapItemtoDLItem(Item item)
		{
			var result = new DL_Item();
			result.Name = item.Name;
			result.Description = item.Description;
			result.CategoryId = item.CategoryId;
			result.Active = item.Active;
			result.Price = item.Price;
			result.Thumbnail = item.Thumbnail;
			result.Timestamp = item.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ItemCategory MapItemCategoryfromDLItemCategory(ADL_ItemCategory itemcategory)
		{
			var result = new ItemCategory();
			result.Id = itemcategory.Id;
			result.Name = itemcategory.Name;
			result.Description = itemcategory.Description;
			result.Active = itemcategory.Active;
			result.VAT = itemcategory.VAT;
			result.Timestamp = itemcategory.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_ItemCategory MapItemCategorytoDLItemCategory(ItemCategory itemcategory)
		{
			var result = new DL_ItemCategory();
			result.Name = itemcategory.Name;
			result.Description = itemcategory.Description;
			result.Active = itemcategory.Active;
			result.VAT = itemcategory.VAT;
			result.Timestamp = itemcategory.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ItemImages MapItemImagesfromDLItemImages(ADL_ItemImages itemimages)
		{
			var result = new ItemImages();
			result.ItemId = itemimages.ItemId;
			result.ImageId = itemimages.ImageId;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_ItemImages MapItemImagestoDLItemImages(ItemImages itemimages)
		{
			var result = new DL_ItemImages();
			result.ItemId = itemimages.ItemId;
			result.ImageId = itemimages.ImageId;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Order MapOrderfromDLOrder(ADL_Order order)
		{
			var result = new Order();
			result.Id = order.Id;
			result.CustomerId = order.CustomerId;
			result.DateOrderPlaced = order.DateOrderPlaced;
			result.DateOrderDispatched = order.DateOrderDispatched;
			result.SourceId = order.SourceId;
			result.Cancelled = order.Cancelled;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Order MapOrdertoDLOrder(Order order)
		{
			var result = new DL_Order();
			result.CustomerId = order.CustomerId;
			result.DateOrderPlaced = order.DateOrderPlaced;
			result.DateOrderDispatched = order.DateOrderDispatched;
			result.SourceId = order.SourceId;
			result.Cancelled = order.Cancelled;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override OrderLine MapOrderLinefromDLOrderLine(ADL_OrderLine orderline)
		{
			var result = new OrderLine();
			result.OrderId = orderline.OrderId;
			result.ItemId = orderline.ItemId;
			result.Quantity = orderline.Quantity;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_OrderLine MapOrderLinetoDLOrderLine(OrderLine orderline)
		{
			var result = new DL_OrderLine();
			result.OrderId = orderline.OrderId;
			result.ItemId = orderline.ItemId;
			result.Quantity = orderline.Quantity;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override PreviousPassword MapPreviousPasswordfromDLPreviousPassword(ADL_PreviousPassword previouspassword)
		{
			var result = new PreviousPassword();
			result.CustomerId = previouspassword.CustomerId;
			result.CreationDate = previouspassword.CreationDate;
			result.Salt = previouspassword.Salt;
			result.PasswordHash = previouspassword.PasswordHash;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_PreviousPassword MapPreviousPasswordtoDLPreviousPassword(PreviousPassword previouspassword)
		{
			var result = new DL_PreviousPassword();
			result.CustomerId = previouspassword.CustomerId;
			result.CreationDate = previouspassword.CreationDate;
			result.Salt = previouspassword.Salt;
			result.PasswordHash = previouspassword.PasswordHash;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Source MapSourcefromDLSource(ADL_Source source)
		{
			var result = new Source();
			result.Id = source.Id;
			result.Description = source.Description;
			result.Name = source.Name;
			result.Timestamp = source.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Source MapSourcetoDLSource(Source source)
		{
			var result = new DL_Source();
			result.Description = source.Description;
			result.Name = source.Name;
			result.Timestamp = source.Timestamp;

			return result;
		}
	}
	[ExcludeFromCodeCoverage]
	public class Mock_DLMapping : ADL_Mapping	{

		[ExcludeFromCodeCoverage]
		public override BasketItem MapBasketItemfromDLBasketItem(ADL_BasketItem basketitem)
		{
			var result = new BasketItem();
			result.CustomerId = basketitem.CustomerId;
			result.ItemId = basketitem.ItemId;
			result.Number = basketitem.Number;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_BasketItem MapBasketItemtoDLBasketItem(BasketItem basketitem)
		{
			var result = new MOCK_DL_BasketItem();
			result.CustomerId = basketitem.CustomerId;
			result.ItemId = basketitem.ItemId;
			result.Number = basketitem.Number;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override CurrentItem MapCurrentItemfromDLCurrentItem(ADL_CurrentItem currentitem)
		{
			var result = new CurrentItem();
			result.Id = currentitem.Id;
			result.Name = currentitem.Name;
			result.Description = currentitem.Description;
			result.CategoryId = currentitem.CategoryId;
			result.Price = currentitem.Price;
			result.Thumbnail = currentitem.Thumbnail;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_CurrentItem MapCurrentItemtoDLCurrentItem(CurrentItem currentitem)
		{
			var result = new MOCK_DL_CurrentItem();
			result.Name = currentitem.Name;
			result.Description = currentitem.Description;
			result.CategoryId = currentitem.CategoryId;
			result.Price = currentitem.Price;
			result.Thumbnail = currentitem.Thumbnail;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override CurrentItemCategory MapCurrentItemCategoryfromDLCurrentItemCategory(ADL_CurrentItemCategory currentitemcategory)
		{
			var result = new CurrentItemCategory();
			result.Id = currentitemcategory.Id;
			result.Name = currentitemcategory.Name;
			result.Description = currentitemcategory.Description;
			result.VAT = currentitemcategory.VAT;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_CurrentItemCategory MapCurrentItemCategorytoDLCurrentItemCategory(CurrentItemCategory currentitemcategory)
		{
			var result = new MOCK_DL_CurrentItemCategory();
			result.Name = currentitemcategory.Name;
			result.Description = currentitemcategory.Description;
			result.VAT = currentitemcategory.VAT;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Customer MapCustomerfromDLCustomer(ADL_Customer customer)
		{
			var result = new Customer();
			result.Id = customer.Id;
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
			result.UserName = customer.UserName;
			result.Salt = customer.Salt;
			result.PasswordHash = customer.PasswordHash;
			result.PasswordNeedsChanging = customer.PasswordNeedsChanging;
			result.Timestamp = customer.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Customer MapCustomertoDLCustomer(Customer customer)
		{
			var result = new MOCK_DL_Customer();
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
			result.UserName = customer.UserName;
			result.Salt = customer.Salt;
			result.PasswordHash = customer.PasswordHash;
			result.PasswordNeedsChanging = customer.PasswordNeedsChanging;
			result.Timestamp = customer.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Image MapImagefromDLImage(ADL_Image image)
		{
			var result = new Image();
			result.Id = image.Id;
			result.ImageContent = image.ImageContent;
			result.ImageDescription = image.ImageDescription;
			result.Timestamp = image.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Image MapImagetoDLImage(Image image)
		{
			var result = new MOCK_DL_Image();
			result.ImageContent = image.ImageContent;
			result.ImageDescription = image.ImageDescription;
			result.Timestamp = image.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Item MapItemfromDLItem(ADL_Item item)
		{
			var result = new Item();
			result.Id = item.Id;
			result.Name = item.Name;
			result.Description = item.Description;
			result.CategoryId = item.CategoryId;
			result.Active = item.Active;
			result.Price = item.Price;
			result.Thumbnail = item.Thumbnail;
			result.Timestamp = item.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Item MapItemtoDLItem(Item item)
		{
			var result = new MOCK_DL_Item();
			result.Name = item.Name;
			result.Description = item.Description;
			result.CategoryId = item.CategoryId;
			result.Active = item.Active;
			result.Price = item.Price;
			result.Thumbnail = item.Thumbnail;
			result.Timestamp = item.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ItemCategory MapItemCategoryfromDLItemCategory(ADL_ItemCategory itemcategory)
		{
			var result = new ItemCategory();
			result.Id = itemcategory.Id;
			result.Name = itemcategory.Name;
			result.Description = itemcategory.Description;
			result.Active = itemcategory.Active;
			result.VAT = itemcategory.VAT;
			result.Timestamp = itemcategory.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_ItemCategory MapItemCategorytoDLItemCategory(ItemCategory itemcategory)
		{
			var result = new MOCK_DL_ItemCategory();
			result.Name = itemcategory.Name;
			result.Description = itemcategory.Description;
			result.Active = itemcategory.Active;
			result.VAT = itemcategory.VAT;
			result.Timestamp = itemcategory.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ItemImages MapItemImagesfromDLItemImages(ADL_ItemImages itemimages)
		{
			var result = new ItemImages();
			result.ItemId = itemimages.ItemId;
			result.ImageId = itemimages.ImageId;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_ItemImages MapItemImagestoDLItemImages(ItemImages itemimages)
		{
			var result = new MOCK_DL_ItemImages();
			result.ItemId = itemimages.ItemId;
			result.ImageId = itemimages.ImageId;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Order MapOrderfromDLOrder(ADL_Order order)
		{
			var result = new Order();
			result.Id = order.Id;
			result.CustomerId = order.CustomerId;
			result.DateOrderPlaced = order.DateOrderPlaced;
			result.DateOrderDispatched = order.DateOrderDispatched;
			result.SourceId = order.SourceId;
			result.Cancelled = order.Cancelled;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Order MapOrdertoDLOrder(Order order)
		{
			var result = new MOCK_DL_Order();
			result.CustomerId = order.CustomerId;
			result.DateOrderPlaced = order.DateOrderPlaced;
			result.DateOrderDispatched = order.DateOrderDispatched;
			result.SourceId = order.SourceId;
			result.Cancelled = order.Cancelled;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override OrderLine MapOrderLinefromDLOrderLine(ADL_OrderLine orderline)
		{
			var result = new OrderLine();
			result.OrderId = orderline.OrderId;
			result.ItemId = orderline.ItemId;
			result.Quantity = orderline.Quantity;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_OrderLine MapOrderLinetoDLOrderLine(OrderLine orderline)
		{
			var result = new MOCK_DL_OrderLine();
			result.OrderId = orderline.OrderId;
			result.ItemId = orderline.ItemId;
			result.Quantity = orderline.Quantity;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override PreviousPassword MapPreviousPasswordfromDLPreviousPassword(ADL_PreviousPassword previouspassword)
		{
			var result = new PreviousPassword();
			result.CustomerId = previouspassword.CustomerId;
			result.CreationDate = previouspassword.CreationDate;
			result.Salt = previouspassword.Salt;
			result.PasswordHash = previouspassword.PasswordHash;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_PreviousPassword MapPreviousPasswordtoDLPreviousPassword(PreviousPassword previouspassword)
		{
			var result = new MOCK_DL_PreviousPassword();
			result.CustomerId = previouspassword.CustomerId;
			result.CreationDate = previouspassword.CreationDate;
			result.Salt = previouspassword.Salt;
			result.PasswordHash = previouspassword.PasswordHash;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override Source MapSourcefromDLSource(ADL_Source source)
		{
			var result = new Source();
			result.Id = source.Id;
			result.Description = source.Description;
			result.Name = source.Name;
			result.Timestamp = source.Timestamp;

			return result;
		}

		[ExcludeFromCodeCoverage]
		public override ADL_Source MapSourcetoDLSource(Source source)
		{
			var result = new MOCK_DL_Source();
			result.Description = source.Description;
			result.Name = source.Name;
			result.Timestamp = source.Timestamp;

			return result;
		}
	}}