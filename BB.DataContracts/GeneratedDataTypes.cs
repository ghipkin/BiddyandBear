using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace BB.DataContracts
{
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class BasketItem
	{
		[DataMember]
		public long CustomerId { get; set; }
		[DataMember]
		public long ItemId { get; set; }
		[DataMember]
		public int Number { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class CurrentItem
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public long CategoryId { get; set; }
		[DataMember]
		public decimal Price { get; set; }
		[DataMember]
		public byte[] Thumbnail { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class CurrentItemCategory
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public bool VAT { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Customer
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string Title { get; set; }
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string AddressLine1 { get; set; }
		[DataMember]
		public string AddressLine2 { get; set; }
		[DataMember]
		public string AddressLine3 { get; set; }
		[DataMember]
		public string AddressLine4 { get; set; }
		[DataMember]
		public string PostalCode { get; set; }
		[DataMember]
		public string Country { get; set; }
		[DataMember]
		public string HomePhoneNo { get; set; }
		[DataMember]
		public string MobilePhoneNo { get; set; }
		[DataMember]
		public string EmailAddress { get; set; }
		[DataMember]
		public string UserName { get; set; }
		[DataMember]
		public byte[] Salt { get; set; }
		[DataMember]
		public string PasswordHash { get; set; }
		[DataMember]
		public bool PasswordNeedsChanging { get; set; }
		[DataMember]
		public byte[] Timestamp { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Image
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public byte[] ImageContent { get; set; }
		[DataMember]
		public string ImageDescription { get; set; }
		[DataMember]
		public byte[] Timestamp { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Item
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public long CategoryId { get; set; }
		[DataMember]
		public bool Active { get; set; }
		[DataMember]
		public decimal Price { get; set; }
		[DataMember]
		public byte[] Thumbnail { get; set; }
		[DataMember]
		public byte[] Timestamp { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class ItemCategory
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public bool Active { get; set; }
		[DataMember]
		public bool VAT { get; set; }
		[DataMember]
		public byte[] Timestamp { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class ItemImages
	{
		[DataMember]
		public long ItemId { get; set; }
		[DataMember]
		public long ImageId { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Order
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public long CustomerId { get; set; }
		[DataMember]
		public DateTime DateOrderPlaced { get; set; }
		[DataMember]
		public DateTime DateOrderDispatched { get; set; }
		[DataMember]
		public long SourceId { get; set; }
		[DataMember]
		public bool Cancelled { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class OrderLine
	{
		[DataMember]
		public long OrderId { get; set; }
		[DataMember]
		public long ItemId { get; set; }
		[DataMember]
		public int Quantity { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class PreviousPassword
	{
		[DataMember]
		public long CustomerId { get; set; }
		[DataMember]
		public DateTime CreationDate { get; set; }
		[DataMember]
		public string Salt { get; set; }
		[DataMember]
		public string PasswordHash { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Source
	{
		[DataMember]
		public long Id { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public byte[] Timestamp { get; set; }
	}
}
