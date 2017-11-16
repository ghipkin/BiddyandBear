using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace BB.Web.Models
{
	[ExcludeFromCodeCoverage]
	public class BasketItem
	{
		public long CustomerId { get; set; }
		public long ItemId { get; set; }
		public int Number { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class CurrentItem
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public long CategoryId { get; set; }
		public decimal Price { get; set; }
		public byte[] Thumbnail { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class CurrentItemCategory
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool VAT { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class Customer
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string AddressLine3 { get; set; }
		public string AddressLine4 { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string HomePhoneNo { get; set; }
		public string MobilePhoneNo { get; set; }
		public string EmailAddress { get; set; }
		public string UserName { get; set; }
		public byte[] Salt { get; set; }
		public string PasswordHash { get; set; }
		public bool PasswordNeedsChanging { get; set; }
		public byte[] Timestamp { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class Image
	{
		public long Id { get; set; }
		public byte[] ImageContent { get; set; }
		public string ImageDescription { get; set; }
		public byte[] Timestamp { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class Item
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public long CategoryId { get; set; }
		public bool Active { get; set; }
		public decimal Price { get; set; }
		public byte[] Thumbnail { get; set; }
		public byte[] Timestamp { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class ItemCategory
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }
		public bool VAT { get; set; }
		public byte[] Timestamp { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class ItemImages
	{
		public long ItemId { get; set; }
		public long ImageId { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class Order
	{
		public long Id { get; set; }
		public long CustomerId { get; set; }
		public DateTime DateOrderPlaced { get; set; }
		public DateTime DateOrderDispatched { get; set; }
		public long SourceId { get; set; }
		public bool Cancelled { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class OrderLine
	{
		public long OrderId { get; set; }
		public long ItemId { get; set; }
		public int Quantity { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class PreviousPassword
	{
		public long CustomerId { get; set; }
		public DateTime CreationDate { get; set; }
		public string Salt { get; set; }
		public string PasswordHash { get; set; }
	}
	[ExcludeFromCodeCoverage]
	public class Source
	{
		public long Id { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public byte[] Timestamp { get; set; }
	}
}
