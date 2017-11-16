using System;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using BB.DataLayer.Abstract;

namespace BB.DataLayer
{
	[ExcludeFromCodeCoverage]
	public abstract class ADL_BasketItems : DatabaseRecords
	{
		public List <ADL_BasketItem> BasketItems { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_BasketItems : ADL_BasketItems
	{
		public DL_BasketItems()
		{
			BasketItems = new List<ADL_BasketItem>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_BasketItem> result;
			string SQL;
			var FirstSQL = "Select CustomerId, ItemId, Number"
			+ " FROM BasketItem";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_BasketItem>();
				while(dr.Read())
				{
					var NewRow = new DL_BasketItem();
					NewRow.CustomerId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("CustomerId"));
					NewRow.ItemId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("ItemId"));
					NewRow.Number = dr.GetFieldValue<int>(dr.GetOrdinal("Number"));
					result.Add(NewRow);
				}
			}
			BasketItems = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in BasketItems)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of BasketItem records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_BasketItems : ADL_BasketItems
	{
		public const string ERR_FAILURE = "Failed to save or load BasketItems records";

		public bool Failure{ get; set; }
		public MOCK_DL_BasketItems()
		{
			BasketItems = new List<ADL_BasketItem>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				BasketItems.Add((ADL_BasketItem)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in BasketItems)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_BasketItem : DatabaseRecord
	{
		public long CustomerId { get; set; }
		public long ItemId { get; set; }
		public int Number { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO BasketItem (CustomerId, ItemId, Number)"
					+ " VALUES (" + this.CustomerId + ", " + this.ItemId + ", " + this.Number + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_BasketItem : ADL_BasketItem
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO BasketItem (CustomerId, ItemId, Number)"
					+ " VALUES (" + this.CustomerId + ", " + this.ItemId + ", " + this.Number + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of BasketItem record failed");
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long CustomerIdvalue = (long)parms.Where(x => x.Key == "CustomerId").FirstOrDefault().Value;
			long ItemIdvalue = (long)parms.Where(x => x.Key == "ItemId").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
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
	public class MOCK_DL_BasketItem : ADL_BasketItem
	{
		public const string ERR_SAVE_FAILED = "Could not save BasketItem record.";
		public bool fail { get; set; }
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_CurrentItems : DatabaseRecords
	{
		public List <ADL_CurrentItem> CurrentItems { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_CurrentItems : ADL_CurrentItems
	{
		public DL_CurrentItems()
		{
			CurrentItems = new List<ADL_CurrentItem>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_CurrentItem> result;
			string SQL;
			var FirstSQL = "Select Id, Name, Description, CategoryId, Price, Thumbnail"
			+ " FROM CurrentItem";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_CurrentItem>();
				while(dr.Read())
				{
					var NewRow = new DL_CurrentItem();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
					NewRow.Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
					NewRow.CategoryId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("CategoryId"));
					NewRow.Price = dr.GetFieldValue<decimal>(dr.GetOrdinal("Price"));
					NewRow.Thumbnail = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Thumbnail"));
					result.Add(NewRow);
				}
			}
			CurrentItems = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in CurrentItems)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of CurrentItem records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_CurrentItems : ADL_CurrentItems
	{
		public const string ERR_FAILURE = "Failed to save or load CurrentItems records";

		public bool Failure{ get; set; }
		public MOCK_DL_CurrentItems()
		{
			CurrentItems = new List<ADL_CurrentItem>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				CurrentItems.Add((ADL_CurrentItem)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in CurrentItems)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_CurrentItem : DatabaseRecord
	{
		public long Id { get; internal set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public long CategoryId { get; set; }
		public decimal Price { get; set; }
		public byte[] Thumbnail { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO CurrentItem (Id, Name, Description, CategoryId, Price, Thumbnail)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.CategoryId + ", " + this.Price + ", " + this.Thumbnail + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_CurrentItem : ADL_CurrentItem
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO CurrentItem (Id, Name, Description, CategoryId, Price, Thumbnail)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.CategoryId + ", " + this.Price + ", " + this.Thumbnail + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
			{
				 String SQL = "Select Id, Name, Description, CategoryId, Price, Thumbnail"
					+ "FROM CurrentItem"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
						Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
						CategoryId = dr.GetFieldValue<long>(dr.GetOrdinal("CategoryId"));
						Price = dr.GetFieldValue<decimal>(dr.GetOrdinal("Price"));
						Thumbnail = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Thumbnail"));
					}
				}
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public class MOCK_DL_CurrentItem : ADL_CurrentItem
	{
		public const string ERR_SAVE_FAILED = "Could not save CurrentItem record.";
		public bool fail { get; set; }
		public const long DEFAULT_CURRENTITEM_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_CURRENTITEM_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_CurrentItemCategories : DatabaseRecords
	{
		public List <ADL_CurrentItemCategory> CurrentItemCategories { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_CurrentItemCategories : ADL_CurrentItemCategories
	{
		public DL_CurrentItemCategories()
		{
			CurrentItemCategories = new List<ADL_CurrentItemCategory>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_CurrentItemCategory> result;
			string SQL;
			var FirstSQL = "Select Id, Name, Description, VAT"
			+ " FROM CurrentItemCategory";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_CurrentItemCategory>();
				while(dr.Read())
				{
					var NewRow = new DL_CurrentItemCategory();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
					NewRow.Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
					NewRow.VAT = dr.GetFieldValue<bool>(dr.GetOrdinal("VAT"));
					result.Add(NewRow);
				}
			}
			CurrentItemCategories = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in CurrentItemCategories)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of CurrentItemCategory records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_CurrentItemCategories : ADL_CurrentItemCategories
	{
		public const string ERR_FAILURE = "Failed to save or load CurrentItemCategories records";

		public bool Failure{ get; set; }
		public MOCK_DL_CurrentItemCategories()
		{
			CurrentItemCategories = new List<ADL_CurrentItemCategory>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				CurrentItemCategories.Add((ADL_CurrentItemCategory)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in CurrentItemCategories)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_CurrentItemCategory : DatabaseRecord
	{
		public long Id { get; internal set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool VAT { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO CurrentItemCategory (Id, Name, Description, VAT)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.VAT + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_CurrentItemCategory : ADL_CurrentItemCategory
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO CurrentItemCategory (Id, Name, Description, VAT)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.VAT + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
			{
				 String SQL = "Select Id, Name, Description, VAT"
					+ "FROM CurrentItemCategory"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
						Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
						VAT = dr.GetFieldValue<bool>(dr.GetOrdinal("VAT"));
					}
				}
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public class MOCK_DL_CurrentItemCategory : ADL_CurrentItemCategory
	{
		public const string ERR_SAVE_FAILED = "Could not save CurrentItemCategory record.";
		public bool fail { get; set; }
		public const long DEFAULT_CURRENTITEMCATEGORY_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_CURRENTITEMCATEGORY_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_Customers : DatabaseRecords
	{
		public List <ADL_Customer> Customers { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_Customers : ADL_Customers
	{
		public DL_Customers()
		{
			Customers = new List<ADL_Customer>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_Customer> result;
			string SQL;
			var FirstSQL = "Select Id, Title, FirstName, LastName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, Country, HomePhoneNo, MobilePhoneNo, EmailAddress, UserName, Salt, PasswordHash, PasswordNeedsChanging, Timestamp"
			+ " FROM Customer";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_Customer>();
				while(dr.Read())
				{
					var NewRow = new DL_Customer();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.Title = dr.GetFieldValue<string>(dr.GetOrdinal("Title"));
					NewRow.FirstName = dr.GetFieldValue<string>(dr.GetOrdinal("FirstName"));
					NewRow.LastName = dr.GetFieldValue<string>(dr.GetOrdinal("LastName"));
					NewRow.AddressLine1 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine1"));
					NewRow.AddressLine2 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine2"));
					NewRow.AddressLine3 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine3"));
					NewRow.AddressLine4 = dr.GetFieldValue<string>(dr.GetOrdinal("AddressLine4"));
					NewRow.PostalCode = dr.GetFieldValue<string>(dr.GetOrdinal("PostalCode"));
					NewRow.Country = dr.GetFieldValue<string>(dr.GetOrdinal("Country"));
					NewRow.HomePhoneNo = dr.GetFieldValue<string>(dr.GetOrdinal("HomePhoneNo"));
					NewRow.MobilePhoneNo = dr.GetFieldValue<string>(dr.GetOrdinal("MobilePhoneNo"));
					NewRow.EmailAddress = dr.GetFieldValue<string>(dr.GetOrdinal("EmailAddress"));
					NewRow.UserName = dr.GetFieldValue<string>(dr.GetOrdinal("UserName"));
					NewRow.Salt = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Salt"));
					NewRow.PasswordHash = dr.GetFieldValue<string>(dr.GetOrdinal("PasswordHash"));
					NewRow.PasswordNeedsChanging = dr.GetFieldValue<bool>(dr.GetOrdinal("PasswordNeedsChanging"));
					NewRow.Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					result.Add(NewRow);
				}
			}
			Customers = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in Customers)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of Customer records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Customers : ADL_Customers
	{
		public const string ERR_FAILURE = "Failed to save or load Customers records";

		public bool Failure{ get; set; }
		public MOCK_DL_Customers()
		{
			Customers = new List<ADL_Customer>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				Customers.Add((ADL_Customer)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in Customers)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_Customer : DatabaseRecord
	{
		public long Id { get; internal set; }
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
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Customer (Id, Title, FirstName, LastName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, Country, HomePhoneNo, MobilePhoneNo, EmailAddress, UserName, Salt, PasswordHash, PasswordNeedsChanging, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Title + ", " + this.FirstName + ", " + this.LastName + ", " + this.AddressLine1 + ", " + this.AddressLine2 + ", " + this.AddressLine3 + ", " + this.AddressLine4 + ", " + this.PostalCode + ", " + this.Country + ", " + this.HomePhoneNo + ", " + this.MobilePhoneNo + ", " + this.EmailAddress + ", " + this.UserName + ", " + this.Salt + ", " + this.PasswordHash + ", " + this.PasswordNeedsChanging + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_Customer : ADL_Customer
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Customer (Id, Title, FirstName, LastName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, Country, HomePhoneNo, MobilePhoneNo, EmailAddress, UserName, Salt, PasswordHash, PasswordNeedsChanging, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Title + ", " + this.FirstName + ", " + this.LastName + ", " + this.AddressLine1 + ", " + this.AddressLine2 + ", " + this.AddressLine3 + ", " + this.AddressLine4 + ", " + this.PostalCode + ", " + this.Country + ", " + this.HomePhoneNo + ", " + this.MobilePhoneNo + ", " + this.EmailAddress + ", " + this.UserName + ", " + this.Salt + ", " + this.PasswordHash + ", " + this.PasswordNeedsChanging + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of Customer record failed");
				}
				else
				{
				using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
				using (SqlDataReader dr = cmd2.ExecuteReader())
					{
						while(dr.Read())
						{
							Id = dr.GetInt64(0); 
						}
					}
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
			{
				 String SQL = "Select Id, Title, FirstName, LastName, AddressLine1, AddressLine2, AddressLine3, AddressLine4, PostalCode, Country, HomePhoneNo, MobilePhoneNo, EmailAddress, UserName, Salt, PasswordHash, PasswordNeedsChanging, Timestamp"
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
						Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					}
				}
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Customer : ADL_Customer
	{
		public const string ERR_SAVE_FAILED = "Could not save Customer record.";
		public bool fail { get; set; }
		public const long DEFAULT_CUSTOMER_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_CUSTOMER_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_Images : DatabaseRecords
	{
		public List <ADL_Image> Images { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_Images : ADL_Images
	{
		public DL_Images()
		{
			Images = new List<ADL_Image>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_Image> result;
			string SQL;
			var FirstSQL = "Select Id, ImageContent, ImageDescription, Timestamp"
			+ " FROM Image";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_Image>();
				while(dr.Read())
				{
					var NewRow = new DL_Image();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.ImageContent = dr.GetFieldValue<byte[]>(dr.GetOrdinal("ImageContent"));
					NewRow.ImageDescription = dr.GetFieldValue<string>(dr.GetOrdinal("ImageDescription"));
					NewRow.Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					result.Add(NewRow);
				}
			}
			Images = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in Images)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of Image records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Images : ADL_Images
	{
		public const string ERR_FAILURE = "Failed to save or load Images records";

		public bool Failure{ get; set; }
		public MOCK_DL_Images()
		{
			Images = new List<ADL_Image>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				Images.Add((ADL_Image)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in Images)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_Image : DatabaseRecord
	{
		public long Id { get; internal set; }
		public byte[] ImageContent { get; set; }
		public string ImageDescription { get; set; }
		public byte[] Timestamp { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Image (Id, ImageContent, ImageDescription, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.ImageContent + ", " + this.ImageDescription + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_Image : ADL_Image
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Image (Id, ImageContent, ImageDescription, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.ImageContent + ", " + this.ImageDescription + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of Image record failed");
				}
				else
				{
				using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
				using (SqlDataReader dr = cmd2.ExecuteReader())
					{
						while(dr.Read())
						{
							Id = dr.GetInt64(0); 
						}
					}
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
			{
				 String SQL = "Select Id, ImageContent, ImageDescription, Timestamp"
					+ "FROM Image"
					+ "WHERE Id = Idvalue";
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(SQL, cn))
				using (SqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.HasRows)
					{
						ImageContent = dr.GetFieldValue<byte[]>(dr.GetOrdinal("ImageContent"));
						ImageDescription = dr.GetFieldValue<string>(dr.GetOrdinal("ImageDescription"));
						Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					}
				}
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Image : ADL_Image
	{
		public const string ERR_SAVE_FAILED = "Could not save Image record.";
		public bool fail { get; set; }
		public const long DEFAULT_IMAGE_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_IMAGE_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_Items : DatabaseRecords
	{
		public List <ADL_Item> Items { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_Items : ADL_Items
	{
		public DL_Items()
		{
			Items = new List<ADL_Item>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_Item> result;
			string SQL;
			var FirstSQL = "Select Id, Name, Description, CategoryId, Active, Price, Thumbnail, Timestamp"
			+ " FROM Item";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_Item>();
				while(dr.Read())
				{
					var NewRow = new DL_Item();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
					NewRow.Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
					NewRow.CategoryId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("CategoryId"));
					NewRow.Active = dr.GetFieldValue<bool>(dr.GetOrdinal("Active"));
					NewRow.Price = dr.GetFieldValue<decimal>(dr.GetOrdinal("Price"));
					NewRow.Thumbnail = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Thumbnail"));
					NewRow.Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					result.Add(NewRow);
				}
			}
			Items = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in Items)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of Item records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Items : ADL_Items
	{
		public const string ERR_FAILURE = "Failed to save or load Items records";

		public bool Failure{ get; set; }
		public MOCK_DL_Items()
		{
			Items = new List<ADL_Item>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				Items.Add((ADL_Item)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in Items)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_Item : DatabaseRecord
	{
		public long Id { get; internal set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public long CategoryId { get; set; }
		public bool Active { get; set; }
		public decimal Price { get; set; }
		public byte[] Thumbnail { get; set; }
		public byte[] Timestamp { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Item (Id, Name, Description, CategoryId, Active, Price, Thumbnail, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.CategoryId + ", " + this.Active + ", " + this.Price + ", " + this.Thumbnail + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_Item : ADL_Item
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Item (Id, Name, Description, CategoryId, Active, Price, Thumbnail, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.CategoryId + ", " + this.Active + ", " + this.Price + ", " + this.Thumbnail + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of Item record failed");
				}
				else
				{
				using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
				using (SqlDataReader dr = cmd2.ExecuteReader())
					{
						while(dr.Read())
						{
							Id = dr.GetInt64(0); 
						}
					}
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
			{
				 String SQL = "Select Id, Name, Description, CategoryId, Active, Price, Thumbnail, Timestamp"
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
						CategoryId = dr.GetFieldValue<long>(dr.GetOrdinal("CategoryId"));
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
	public class MOCK_DL_Item : ADL_Item
	{
		public const string ERR_SAVE_FAILED = "Could not save Item record.";
		public bool fail { get; set; }
		public const long DEFAULT_ITEM_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_ITEM_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_ItemCategories : DatabaseRecords
	{
		public List <ADL_ItemCategory> ItemCategories { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_ItemCategories : ADL_ItemCategories
	{
		public DL_ItemCategories()
		{
			ItemCategories = new List<ADL_ItemCategory>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_ItemCategory> result;
			string SQL;
			var FirstSQL = "Select Id, Name, Description, Active, VAT, Timestamp"
			+ " FROM ItemCategory";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_ItemCategory>();
				while(dr.Read())
				{
					var NewRow = new DL_ItemCategory();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
					NewRow.Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
					NewRow.Active = dr.GetFieldValue<bool>(dr.GetOrdinal("Active"));
					NewRow.VAT = dr.GetFieldValue<bool>(dr.GetOrdinal("VAT"));
					NewRow.Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					result.Add(NewRow);
				}
			}
			ItemCategories = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in ItemCategories)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of ItemCategory records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_ItemCategories : ADL_ItemCategories
	{
		public const string ERR_FAILURE = "Failed to save or load ItemCategories records";

		public bool Failure{ get; set; }
		public MOCK_DL_ItemCategories()
		{
			ItemCategories = new List<ADL_ItemCategory>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				ItemCategories.Add((ADL_ItemCategory)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in ItemCategories)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_ItemCategory : DatabaseRecord
	{
		public long Id { get; internal set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }
		public bool VAT { get; set; }
		public byte[] Timestamp { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO ItemCategory (Id, Name, Description, Active, VAT, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.Active + ", " + this.VAT + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_ItemCategory : ADL_ItemCategory
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO ItemCategory (Id, Name, Description, Active, VAT, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Name + ", " + this.Description + ", " + this.Active + ", " + this.VAT + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of ItemCategory record failed");
				}
				else
				{
				using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
				using (SqlDataReader dr = cmd2.ExecuteReader())
					{
						while(dr.Read())
						{
							Id = dr.GetInt64(0); 
						}
					}
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
			{
				 String SQL = "Select Id, Name, Description, Active, VAT, Timestamp"
					+ "FROM ItemCategory"
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
						VAT = dr.GetFieldValue<bool>(dr.GetOrdinal("VAT"));
						Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					}
				}
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public class MOCK_DL_ItemCategory : ADL_ItemCategory
	{
		public const string ERR_SAVE_FAILED = "Could not save ItemCategory record.";
		public bool fail { get; set; }
		public const long DEFAULT_ITEMCATEGORY_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_ITEMCATEGORY_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_ItemImagess : DatabaseRecords
	{
		public List <ADL_ItemImages> ItemImagess { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_ItemImagess : ADL_ItemImagess
	{
		public DL_ItemImagess()
		{
			ItemImagess = new List<ADL_ItemImages>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_ItemImages> result;
			string SQL;
			var FirstSQL = "Select ItemId, ImageId"
			+ " FROM ItemImages";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_ItemImages>();
				while(dr.Read())
				{
					var NewRow = new DL_ItemImages();
					NewRow.ItemId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("ItemId"));
					NewRow.ImageId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("ImageId"));
					result.Add(NewRow);
				}
			}
			ItemImagess = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in ItemImagess)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of ItemImages records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_ItemImagess : ADL_ItemImagess
	{
		public const string ERR_FAILURE = "Failed to save or load ItemImagess records";

		public bool Failure{ get; set; }
		public MOCK_DL_ItemImagess()
		{
			ItemImagess = new List<ADL_ItemImages>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				ItemImagess.Add((ADL_ItemImages)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in ItemImagess)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_ItemImages : DatabaseRecord
	{
		public long ItemId { get; set; }
		public long ImageId { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO ItemImages (ItemId, ImageId)"
					+ " VALUES (" + this.ItemId + ", " + this.ImageId + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_ItemImages : ADL_ItemImages
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO ItemImages (ItemId, ImageId)"
					+ " VALUES (" + this.ItemId + ", " + this.ImageId + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of ItemImages record failed");
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long ItemIdvalue = (long)parms.Where(x => x.Key == "ItemId").FirstOrDefault().Value;
			long ImageIdvalue = (long)parms.Where(x => x.Key == "ImageId").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
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
	public class MOCK_DL_ItemImages : ADL_ItemImages
	{
		public const string ERR_SAVE_FAILED = "Could not save ItemImages record.";
		public bool fail { get; set; }
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_Orders : DatabaseRecords
	{
		public List <ADL_Order> Orders { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_Orders : ADL_Orders
	{
		public DL_Orders()
		{
			Orders = new List<ADL_Order>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_Order> result;
			string SQL;
			var FirstSQL = "Select Id, CustomerId, DateOrderPlaced, DateOrderDispatched, SourceId, Cancelled"
			+ " FROM Order";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_Order>();
				while(dr.Read())
				{
					var NewRow = new DL_Order();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.CustomerId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("CustomerId"));
					NewRow.DateOrderPlaced = dr.GetFieldValue<DateTime>(dr.GetOrdinal("DateOrderPlaced"));
					NewRow.DateOrderDispatched = dr.GetFieldValue<DateTime>(dr.GetOrdinal("DateOrderDispatched"));
					NewRow.SourceId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("SourceId"));
					NewRow.Cancelled = dr.GetFieldValue<bool>(dr.GetOrdinal("Cancelled"));
					result.Add(NewRow);
				}
			}
			Orders = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in Orders)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of Order records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Orders : ADL_Orders
	{
		public const string ERR_FAILURE = "Failed to save or load Orders records";

		public bool Failure{ get; set; }
		public MOCK_DL_Orders()
		{
			Orders = new List<ADL_Order>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				Orders.Add((ADL_Order)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in Orders)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_Order : DatabaseRecord
	{
		public long Id { get; internal set; }
		public long CustomerId { get; set; }
		public DateTime DateOrderPlaced { get; set; }
		public DateTime DateOrderDispatched { get; set; }
		public long SourceId { get; set; }
		public bool Cancelled { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Order (Id, CustomerId, DateOrderPlaced, DateOrderDispatched, SourceId, Cancelled)"
					+ " VALUES (" + this.Id + ", " + this.CustomerId + ", " + this.DateOrderPlaced + ", " + this.DateOrderDispatched + ", " + this.SourceId + ", " + this.Cancelled + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_Order : ADL_Order
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Order (Id, CustomerId, DateOrderPlaced, DateOrderDispatched, SourceId, Cancelled)"
					+ " VALUES (" + this.Id + ", " + this.CustomerId + ", " + this.DateOrderPlaced + ", " + this.DateOrderDispatched + ", " + this.SourceId + ", " + this.Cancelled + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of Order record failed");
				}
				else
				{
				using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
				using (SqlDataReader dr = cmd2.ExecuteReader())
					{
						while(dr.Read())
						{
							Id = dr.GetInt64(0); 
						}
					}
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
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
						CustomerId = dr.GetFieldValue<long>(dr.GetOrdinal("CustomerId"));
						DateOrderPlaced = dr.GetFieldValue<DateTime>(dr.GetOrdinal("DateOrderPlaced"));
						DateOrderDispatched = dr.GetFieldValue<DateTime>(dr.GetOrdinal("DateOrderDispatched"));
						SourceId = dr.GetFieldValue<long>(dr.GetOrdinal("SourceId"));
						Cancelled = dr.GetFieldValue<bool>(dr.GetOrdinal("Cancelled"));
					}
				}
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Order : ADL_Order
	{
		public const string ERR_SAVE_FAILED = "Could not save Order record.";
		public bool fail { get; set; }
		public const long DEFAULT_ORDER_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_ORDER_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_OrderLines : DatabaseRecords
	{
		public List <ADL_OrderLine> OrderLines { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_OrderLines : ADL_OrderLines
	{
		public DL_OrderLines()
		{
			OrderLines = new List<ADL_OrderLine>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_OrderLine> result;
			string SQL;
			var FirstSQL = "Select OrderId, ItemId, Quantity"
			+ " FROM OrderLine";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_OrderLine>();
				while(dr.Read())
				{
					var NewRow = new DL_OrderLine();
					NewRow.OrderId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("OrderId"));
					NewRow.ItemId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("ItemId"));
					NewRow.Quantity = dr.GetFieldValue<int>(dr.GetOrdinal("Quantity"));
					result.Add(NewRow);
				}
			}
			OrderLines = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in OrderLines)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of OrderLine records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_OrderLines : ADL_OrderLines
	{
		public const string ERR_FAILURE = "Failed to save or load OrderLines records";

		public bool Failure{ get; set; }
		public MOCK_DL_OrderLines()
		{
			OrderLines = new List<ADL_OrderLine>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				OrderLines.Add((ADL_OrderLine)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in OrderLines)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_OrderLine : DatabaseRecord
	{
		public long OrderId { get; set; }
		public long ItemId { get; set; }
		public int Quantity { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO OrderLine (OrderId, ItemId, Quantity)"
					+ " VALUES (" + this.OrderId + ", " + this.ItemId + ", " + this.Quantity + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_OrderLine : ADL_OrderLine
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO OrderLine (OrderId, ItemId, Quantity)"
					+ " VALUES (" + this.OrderId + ", " + this.ItemId + ", " + this.Quantity + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of OrderLine record failed");
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long OrderIdvalue = (long)parms.Where(x => x.Key == "OrderId").FirstOrDefault().Value;
			long ItemIdvalue = (long)parms.Where(x => x.Key == "ItemId").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
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
	public class MOCK_DL_OrderLine : ADL_OrderLine
	{
		public const string ERR_SAVE_FAILED = "Could not save OrderLine record.";
		public bool fail { get; set; }
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_PreviousPasswords : DatabaseRecords
	{
		public List <ADL_PreviousPassword> PreviousPasswords { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_PreviousPasswords : ADL_PreviousPasswords
	{
		public DL_PreviousPasswords()
		{
			PreviousPasswords = new List<ADL_PreviousPassword>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_PreviousPassword> result;
			string SQL;
			var FirstSQL = "Select CustomerId, CreationDate, Salt, PasswordHash"
			+ " FROM PreviousPassword";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_PreviousPassword>();
				while(dr.Read())
				{
					var NewRow = new DL_PreviousPassword();
					NewRow.CustomerId = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("CustomerId"));
					NewRow.CreationDate = dr.GetFieldValue<DateTime>(dr.GetOrdinal("CreationDate"));
					NewRow.Salt = dr.GetFieldValue<string>(dr.GetOrdinal("Salt"));
					NewRow.PasswordHash = dr.GetFieldValue<string>(dr.GetOrdinal("PasswordHash"));
					result.Add(NewRow);
				}
			}
			PreviousPasswords = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in PreviousPasswords)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of PreviousPassword records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_PreviousPasswords : ADL_PreviousPasswords
	{
		public const string ERR_FAILURE = "Failed to save or load PreviousPasswords records";

		public bool Failure{ get; set; }
		public MOCK_DL_PreviousPasswords()
		{
			PreviousPasswords = new List<ADL_PreviousPassword>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				PreviousPasswords.Add((ADL_PreviousPassword)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in PreviousPasswords)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_PreviousPassword : DatabaseRecord
	{
		public long CustomerId { get; set; }
		public DateTime CreationDate { get; set; }
		public string Salt { get; set; }
		public string PasswordHash { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO PreviousPassword (CustomerId, CreationDate, Salt, PasswordHash)"
					+ " VALUES (" + this.CustomerId + ", " + this.CreationDate + ", " + this.Salt + ", " + this.PasswordHash + ");";
			}
			return InsertSQL;
		}

	}

	[ExcludeFromCodeCoverage]
	public class DL_PreviousPassword : ADL_PreviousPassword
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO PreviousPassword (CustomerId, CreationDate, Salt, PasswordHash)"
					+ " VALUES (" + this.CustomerId + ", " + this.CreationDate + ", " + this.Salt + ", " + this.PasswordHash + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of PreviousPassword record failed");
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long CustomerIdvalue = (long)parms.Where(x => x.Key == "CustomerId").FirstOrDefault().Value;
			DateTime CreationDatevalue = (DateTime)parms.Where(x => x.Key == "CreationDate").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
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
	public class MOCK_DL_PreviousPassword : ADL_PreviousPassword
	{
		public const string ERR_SAVE_FAILED = "Could not save PreviousPassword record.";
		public bool fail { get; set; }
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

	[ExcludeFromCodeCoverage]
	public abstract class ADL_Sources : DatabaseRecords
	{
		public List <ADL_Source> Sources { get; protected set; }
	}


	[ExcludeFromCodeCoverage]
	public class DL_Sources : ADL_Sources
	{
		public DL_Sources()
		{
			Sources = new List<ADL_Source>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			List<ADL_Source> result;
			string SQL;
			var FirstSQL = "Select Id, Description, Name, Timestamp"
			+ " FROM Source";
			if(WhereParams != null && WhereParams.Count > 0)
			{
				var sbSQL = new StringBuilder();
				sbSQL.Append(FirstSQL);
				sbSQL.Append(" WHERE ");
				foreach(var param in WhereParams)
				{
					sbSQL.AppendLine(param.Key);
					sbSQL.Append(" = ");
					sbSQL.Append(param.Value);
					sbSQL.AppendLine(" AND");
				}
				sbSQL.Remove(sbSQL.Length-2,2);
				SQL = sbSQL.ToString();
			}
			else
			{
				SQL = FirstSQL;
			}
			using (SqlConnection cn = GetSQLConnection())
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				result = new List<ADL_Source>();
				while(dr.Read())
				{
					var NewRow = new DL_Source();
					NewRow.Id = (long)dr.GetFieldValue<decimal>(dr.GetOrdinal("Id"));
					NewRow.Description = dr.GetFieldValue<string>(dr.GetOrdinal("Description"));
					NewRow.Name = dr.GetFieldValue<string>(dr.GetOrdinal("Name"));
					NewRow.Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					result.Add(NewRow);
				}
			}
			Sources = result;
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			bool NeedtoCloseConnection = false;

			var InsertSQL = new StringBuilder();
			foreach(var record in Sources)
			{
				InsertSQL.Append(record.GetInsertSQL());
				InsertSQL.Append(";");
			}

			if(cn == null)
			{
				cn = GetSQLConnection();
				NeedtoCloseConnection = true;
			}

			using (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))
			{
				var SaveResult = cmd.ExecuteNonQuery();
				if(SaveResult < 1)
				{
					throw new Exception("Save of Source records failed.");
				}
			}

			if(NeedtoCloseConnection)
			{
				cn.Close();
				cn.Dispose();
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Sources : ADL_Sources
	{
		public const string ERR_FAILURE = "Failed to save or load Sources records";

		public bool Failure{ get; set; }
		public MOCK_DL_Sources()
		{
			Sources = new List<ADL_Source>();
		}

		public override void LoadRecords(Dictionary<String, Object> WhereParams)
		{
			if(Failure)
			{
				throw new Exception(ERR_FAILURE);
			}
			foreach(var record in BB.Mocks.MockDatabase.MockedDb)
			{
				Sources.Add((ADL_Source)record);
			}
		}

		public override void SaveRecords(SqlConnection cn = null)
		{
			foreach(var record in Sources)
			{
				if(Failure)
				{
					throw new Exception(ERR_FAILURE);
				}
				BB.Mocks.MockDatabase.Insert(record);
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public abstract class ADL_Source : DatabaseRecord
	{
		public long Id { get; internal set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public byte[] Timestamp { get; set; }
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Source (Id, Description, Name, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Description + ", " + this.Name + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}

	}


	[ExcludeFromCodeCoverage]
	public class DL_Source : ADL_Source
	{
		public override string GetInsertSQL()
		{
			if(String.IsNullOrEmpty(InsertSQL))
			{
				InsertSQL = "INSERT INTO Source (Id, Description, Name, Timestamp)"
					+ " VALUES (" + this.Id + ", " + this.Description + ", " + this.Name + ", " + this.Timestamp + ");";
			}
			return InsertSQL;
		}
		public override void Save(SqlConnection cn = null)
		{
			var SQL = GetInsertSQL();
			bool NeedtoCloseConnection = false;
			if(cn == null)
			{
				cn = GetSQLConnection();
				cn.Open();
				NeedtoCloseConnection = true;
			}
			using (SqlCommand cmd = new SqlCommand(SQL, cn))
			{
				var result = cmd.ExecuteNonQuery();
				if(result != 1)
				{
					throw new Exception("Insert of Source record failed");
				}
				else
				{
				using (SqlCommand cmd2 = new SqlCommand("SELECT @@IDENTITY;"))
				using (SqlDataReader dr = cmd2.ExecuteReader())
					{
						while(dr.Read())
						{
							Id = dr.GetInt64(0); 
						}
					}
				}
			}
			if(NeedtoCloseConnection);
			{
				cn.Close();
				cn.Dispose();
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
			long Idvalue = (long)parms.Where(x => x.Key == "Id").FirstOrDefault().Value;
			using (SqlConnection cn = GetSQLConnection())
			{
				 String SQL = "Select Id, Description, Name, Timestamp"
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
						Timestamp = dr.GetFieldValue<byte[]>(dr.GetOrdinal("Timestamp"));
					}
				}
			}
		}
	}


	[ExcludeFromCodeCoverage]
	public class MOCK_DL_Source : ADL_Source
	{
		public const string ERR_SAVE_FAILED = "Could not save Source record.";
		public bool fail { get; set; }
		public const long DEFAULT_SOURCE_ID = 595168639;
		public override string GetInsertSQL()
		{
			return String.Empty;
		}

		public override void Save(SqlConnection cn = null)
		{
			if(fail)
			{
				throw new Exception(ERR_SAVE_FAILED);
			}
			else
			{
				this.Id = DEFAULT_SOURCE_ID;
				BB.Mocks.MockDatabase.Insert(this);
			}
		}

		public override void Load(Dictionary<string, object> parms)
		{
		}
	}

}
