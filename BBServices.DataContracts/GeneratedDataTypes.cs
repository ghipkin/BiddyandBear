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
		public decimal CustomerId { get; set; }
		[DataMember]
		public decimal ItemId { get; set; }
		[DataMember]
		public int Number { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Customer
	{
		[DataMember]
		public decimal Id { get; set; }
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
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Image
	{
		[DataMember]
		public byte[] Image { get; set; }
		[DataMember]
		public string ImageDescription { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Item
	{
		[DataMember]
		public decimal Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
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
	public class Order
	{
		[DataMember]
		public DateTime DateOrderPlaced { get; set; }
		[DataMember]
		public DateTime DateOrderDispatched { get; set; }
		[DataMember]
		public decimal SourceId { get; set; }
		[DataMember]
		public bool Cancelled { get; set; }
	}
	[DataContract]
	[ExcludeFromCodeCoverage]
	public class Source
	{
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public string Name { get; set; }
	}
}
