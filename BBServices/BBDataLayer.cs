using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace BB.DataLayer
{
[ExcludeFromCodeCoverage]
	public class DL_BasketItem : IDatabaseRecord
	{
		public decimal CustomerId { get; set; }
		public decimal ItemId { get; set; }
		public int Number { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO BasketItem (CustomerId, ItemId, Number)"
					+ " VALUES (CustomerId, ItemId, Number);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal CustomerIdvalue = (decimal)parms.Where(x => x.Key == "CustomerId").FirstOrDefault().Value;
			decimal ItemIdvalue = (decimal)parms.Where(x => x.Key == "ItemId").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select CustomerId, ItemId, Number"
					+ "FROM BasketItem"
					+ "WHERE CustomerId = CustomerIdvalue, ItemId = ItemIdvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Number = dr.GetFieldValue<int>(dr.GetOrdinal("Number"));
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_Customer : IDatabaseRecord
	{
		public decimal Id { get; private set; }
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

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO Customer (Id, Title, FirstName, LastName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, Country, HomePhoneNo, MobilePhoneNo, EmailAddress, UserName, Salt, PasswordHash, PasswordNeedsChanging)"
					+ " VALUES (Id, Title, FirstName, LastName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, Country, HomePhoneNo, MobilePhoneNo, EmailAddress, UserName, Salt, PasswordHash, PasswordNeedsChanging);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
					else
					{
					using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
					using (SqlDataReader dr = cmd2.ExecuteReader())
						{
							while(dr.Read())
							{
								Id = dr.GetDecimal(0); 
							}
						}
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal Idvalue = (decimal)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select Id, Title, FirstName, LastName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, Country, HomePhoneNo, MobilePhoneNo, EmailAddress, UserName, Salt, PasswordHash, PasswordNeedsChanging"
					+ "FROM Customer"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Title = dr.GetFieldValue<string>(dr.GetOrdinal("Title"));
						FirstName = dr.GetFieldValue<string>(dr.GetOrdinal("FirstName"));
						LastName = dr.GetFieldValue<string>(dr.GetOrdinal("LastName"));
						AddressLine1 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine1"));
						AddressLine2 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine2"));
						AddressLine3 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine3"));
						AddressLine4 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine4"));
						PostalCode = dr.GetFieldValue<string>(dr.GetOrdinal("PostalCode"));
						Country = dr.GetFieldValue<string>(dr.GetOrdinal("Country"));
						HomePhoneNo = dr.GetFieldValue<string>(dr.GetOrdinal("HomePhoneNo"));
						MobilePhoneNo = dr.GetFieldValue<string>(dr.GetOrdinal("MobilePhoneNo"));
						EmailAddress = dr.GetFieldValue<string>(dr.GetOrdinal("EmailAddress"));
						UserName = dr.GetFieldValue<string>(dr.GetOrdinal("UserName"));
						Salt = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Salt"));
						PasswordHash = dr.GetFieldValue<string>(dr.GetOrdinal("PasswordHash"));
						PasswordNeedsChanging = dr.GetFieldValue<bool>(dr.GetOrdinal("PasswordNeedsChanging"));
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_Image : IDatabaseRecord
	{
		public int Id { get; private set; }
		public byte[] Image { get; set; }
		public string ImageDescription { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO Image (Id, Image, ImageDescription)"
					+ " VALUES (Id, Image, ImageDescription);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
					else
					{
					using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
					using (SqlDataReader dr = cmd2.ExecuteReader())
						{
							while(dr.Read())
							{
								Id = dr.GetDecimal(0); 
							}
						}
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			int Idvalue = (int)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select Id, Image, ImageDescription"
					+ "FROM Image"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Image = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Image"));
						ImageDescription = dr.GetFieldValue<string>(dr.GetOrdinal("ImageDescription"));
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_Item : IDatabaseRecord
	{
		public decimal Id { get; private set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }
		public decimal Price { get; set; }
		public byte[] Thumbnail { get; set; }
		public byte[] Timestamp { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO Item (Id, Name, Description, Active, Price, Thumbnail, Timestamp)"
					+ " VALUES (Id, Name, Description, Active, Price, Thumbnail, Timestamp);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
					else
					{
					using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
					using (SqlDataReader dr = cmd2.ExecuteReader())
						{
							while(dr.Read())
							{
								Id = dr.GetDecimal(0); 
							}
						}
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal Idvalue = (decimal)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select Id, Name, Description, Active, Price, Thumbnail, Timestamp"
					+ "FROM Item"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
						Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
						Active = dr.GetFieldValue<bool>(dr.GetOrdinal("Active"));
						Price = dr.GetFieldValue<decimal>(dr.GetOrdinal("Price"));
						Thumbnail = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Thumbnail"));
						Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_ItemImages : IDatabaseRecord
	{
		public decimal ItemId { get; set; }
		public decimal ImageId { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO ItemImages (ItemId, ImageId)"
					+ " VALUES (ItemId, ImageId);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal ItemIdvalue = (decimal)parms.Where(x => x.Key == "ItemId").FirstOrDefault().Value;
			decimal ImageIdvalue = (decimal)parms.Where(x => x.Key == "ImageId").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select ItemId, ImageId"
					+ "FROM ItemImages"
					+ "WHERE ItemId = ItemIdvalue, ImageId = ImageIdvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_Order : IDatabaseRecord
	{
		public decimal Id { get; private set; }
		public decimal CustomerId { get; set; }
		public DateTime DateOrderPlaced { get; set; }
		public DateTime DateOrderDispatched { get; set; }
		public decimal SourceId { get; set; }
		public bool Cancelled { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO Order (Id, CustomerId, DateOrderPlaced, DateOrderDispatched, SourceId, Cancelled)"
					+ " VALUES (Id, CustomerId, DateOrderPlaced, DateOrderDispatched, SourceId, Cancelled);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
					else
					{
					using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
					using (SqlDataReader dr = cmd2.ExecuteReader())
						{
							while(dr.Read())
							{
								Id = dr.GetDecimal(0); 
							}
						}
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal Idvalue = (decimal)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select Id, CustomerId, DateOrderPlaced, DateOrderDispatched, SourceId, Cancelled"
					+ "FROM Order"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						CustomerId = dr.GetFieldValue<decimal>(dr.GetOrdinal("CustomerId"));
						DateOrderPlaced = dr.GetFieldValue<DateTime>(dr.GetOrdinal("DateOrderPlaced"));
						DateOrderDispatched = dr.GetFieldValue<DateTime>(dr.GetOrdinal("DateOrderDispatched"));
						SourceId = dr.GetFieldValue<decimal>(dr.GetOrdinal("SourceId"));
						Cancelled = dr.GetFieldValue<bool>(dr.GetOrdinal("Cancelled"));
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_OrderLine : IDatabaseRecord
	{
		public decimal OrderId { get; set; }
		public decimal ItemId { get; set; }
		public int Quantity { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO OrderLine (OrderId, ItemId, Quantity)"
					+ " VALUES (OrderId, ItemId, Quantity);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal OrderIdvalue = (decimal)parms.Where(x => x.Key == "OrderId").FirstOrDefault().Value;
			decimal ItemIdvalue = (decimal)parms.Where(x => x.Key == "ItemId").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select OrderId, ItemId, Quantity"
					+ "FROM OrderLine"
					+ "WHERE OrderId = OrderIdvalue, ItemId = ItemIdvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Quantity = dr.GetFieldValue<int>(dr.GetOrdinal("Quantity"));
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_PreviousPassword : IDatabaseRecord
	{
		public decimal CustomerId { get; set; }
		public DateTime CreationDate { get; set; }
		public string Salt { get; set; }
		public string PasswordHash { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO PreviousPassword (CustomerId, CreationDate, Salt, PasswordHash)"
					+ " VALUES (CustomerId, CreationDate, Salt, PasswordHash);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal CustomerIdvalue = (decimal)parms.Where(x => x.Key == "CustomerId").FirstOrDefault().Value;
			DateTime CreationDatevalue = (DateTime)parms.Where(x => x.Key == "CreationDate").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select CustomerId, CreationDate, Salt, PasswordHash"
					+ "FROM PreviousPassword"
					+ "WHERE CustomerId = CustomerIdvalue, CreationDate = CreationDatevalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Salt = dr.GetFieldValue<string>(dr.GetOrdinal("Salt"));
						PasswordHash = dr.GetFieldValue<string>(dr.GetOrdinal("PasswordHash"));
					}
				}
			}
		}
	}

[ExcludeFromCodeCoverage]
	public class DL_Source : IDatabaseRecord
	{
		public decimal Id { get; private set; }
		public string Description { get; set; }
		public string Name { get; set; }

		public void Save()
		{
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
			{
				 String SQL = "INSERT INTO Source (Id, Description, Name)"
					+ " VALUES (Id, Description, Name);";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				{
					var result = cmd.ExecuteNonQuery();
					if(result != 1)
					{
						throw new Exception("Insert of record failed");
					}
					else
					{
					using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
					using (SqlDataReader dr = cmd2.ExecuteReader())
						{
							while(dr.Read())
							{
								Id = dr.GetDecimal(0); 
							}
						}
					}
				}
			}
		}

		public void Load(Dictionary<string, object> parms)
		{
			decimal Idvalue = (decimal)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DEV"].ConnectionString))
			{
				 String SQL = "Select Id, Description, Name"
					+ "FROM Source"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
						Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
					}
				}
			}
		}
	}
}
