using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenerateDataLayer
{
    public partial class GenerateCode : Form
    {
        public GenerateCode()
        {
            InitializeComponent();
        }

        private void GenerateDataLayer_Click(object sender, EventArgs e)
        {
            StringBuilder sb = null;
            List<DatabaseField> DatabaseFields = GetAllDatabaseFields();
            if (DatabaseFields.Count >0)
            {
                
                sb = new StringBuilder();

                sb.AppendLine("using System;");
                sb.AppendLine("using System.Text;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.Configuration;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Data;");
                sb.AppendLine("using System.Data.SqlClient;");
                sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
                sb.AppendLine();
                sb.AppendLine("namespace BB.DataLayer");
                sb.AppendLine("{");

                string CurrentTable = string.Empty;
                foreach(var Field in DatabaseFields)
                {
                    if (CurrentTable != Field.TableName)
                    {
                        if (!string.IsNullOrEmpty(CurrentTable))
                        {
                            sb.AppendLine();
                            //Add the overriding methods for the previous class
                            AppendOverridingMethods(CurrentTable, DatabaseFields.Where( x=>x.TableName == CurrentTable).ToList<DatabaseField>(), sb);
                            sb.AppendLine("\t}");
                            sb.AppendLine();
                        }
                        //Create Collection class
                        GetCollectionClass(Field.TableName, DatabaseFields.Where(x=>x.TableName==Field.TableName).ToList<DatabaseField>() , sb);

                        sb.AppendLine("\t[ExcludeFromCodeCoverage]");
                        sb.Append("\tpublic class ");
                        sb.Append("DL_");
                        sb.Append(Field.TableName);
                        sb.AppendLine(" : IDatabaseRecord");
                        sb.AppendLine("\t{");
                    }
                    sb.Append("\t\tpublic ");
                    sb.Append(GetDotNetDataType(Field.SQLDataType));
                    sb.Append(" ");
                    sb.Append(Field.FieldName);
                    sb.Append(" { get;");
                    if (Field.SQLDataType == "Timestamp" || Field.IsIdentity)
                    {
                        sb.Append(" private");
                    }
                    sb.Append(" set; }");
                    sb.AppendLine();

                    CurrentTable = Field.TableName;
                }
                sb.AppendLine();
                //Add the overriding methods for the previous class
                AppendOverridingMethods(CurrentTable, DatabaseFields.Where(x => x.TableName == CurrentTable).ToList<DatabaseField>(), sb);


                sb.AppendLine("\t}");
                sb.AppendLine("}");

                string filename = Application.StartupPath + "\\..\\..\\..\\BBServices\\BBDataLayer.cs";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (var fs = new FileStream(filename, FileMode.CreateNew))
                {
                    fs.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.ToString().Length);
                    fs.Flush();
                }

            }
        }

        private void GetCollectionClass(string TableName, List<DatabaseField> Fields, StringBuilder sb)
        {
            sb.AppendLine("\t[ExcludeFromCodeCoverage]");
            sb.Append("\tpublic class ");
            sb.Append("DL_");
            sb.Append(TableName);
            sb.AppendLine("s : IDatabaseRecords");
            sb.AppendLine("\t{");
            sb.AppendLine();
            sb.AppendLine("\t\tpublic List<IDatabaseRecord> LoadRecords(Dictionary<String, Object> WhereParams)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn Load(WhereParams).ConvertAll(x=>(IDatabaseRecord)x);");
            sb.AppendLine("\t\t}");
            sb.AppendLine();
            sb.Append("\t\tpublic List<DL_");
            sb.Append(TableName);
            sb.AppendLine("> Load(Dictionary<String, Object> WhereParams)");
            sb.AppendLine("\t\t{");
            sb.Append("\t\t\tList<DL_");
            sb.Append(TableName);
            sb.AppendLine("> result;");
            sb.Append("\t\t\tvar SQL = \"Select ");
            var Fieldnames = new StringBuilder();
            foreach(var field in Fields)
            {
                Fieldnames.Append(field.FieldName);
                Fieldnames.Append(", ");
            }
            sb.Append(Fieldnames.ToString().Substring(0, Fieldnames.ToString().Length - 2));
            sb.AppendLine("\"");
            sb.Append("\t\t\t+ \"FROM ");
            sb.Append(TableName);
            sb.AppendLine("\"");
            sb.AppendLine("\t\t\t+ \" WHERE \";");
            sb.AppendLine("\t\t\tvar sbSQL = new StringBuilder();");
            sb.AppendLine("\t\t\tsbSQL.Append(SQL);");
            sb.AppendLine("\t\t\tforeach(var param in WhereParams)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tsbSQL.AppendLine(param.Key);");
            sb.AppendLine("\t\t\t\tsbSQL.Append(\" = \");");
            sb.AppendLine("\t\t\t\tsbSQL.Append(param.Value);");
            sb.AppendLine("\t\t\t\tsbSQL.AppendLine(\" AND\");");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tusing (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"BBcn\"].ConnectionString))");
            sb.AppendLine("\t\t\tusing (SqlCommand cmd = new SqlCommand(sbSQL.ToString().Substring(0, sbSQL.ToString().Length -4)))");
            sb.AppendLine("\t\t\tusing (SqlDataReader dr = cmd.ExecuteReader())");
            sb.AppendLine("\t\t\t{");
            sb.Append("\t\t\t\tresult = new List<DL_");
            sb.Append(TableName);
            sb.AppendLine(">();");
            sb.AppendLine("\t\t\t\twhile(dr.Read())");
            sb.AppendLine("\t\t\t\t{");
            sb.Append("\t\t\t\t\tvar NewRow = new DL_");
            sb.Append(TableName);
            sb.AppendLine("();");
            foreach(var field in Fields)
            {
                sb.Append("\t\t\t\t\tNewRow.");
                sb.Append(field.FieldName);
                sb.Append(" = dr.GetFieldValue<");
                sb.Append(GetDotNetDataType(field.SQLDataType));
                sb.Append(">(dr.GetOrdinal(\"");
                sb.Append(field.FieldName);
                sb.AppendLine("\"));");
            }
            sb.AppendLine("\t\t\t\t\tresult.Add(NewRow);");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn result;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
        }

        private List<DatabaseField> GetAllDatabaseFields()
        {
            string SQL = "Select c.TABLE_NAME, c.COLUMN_NAME, cast(iif(x.CONSTRAINT_TYPE = 'PRIMARY KEY', 1, 0) as bit) as PrimaryKey"
                + ", cast(COLUMNPROPERTY(object_id(c.TABLE_SCHEMA +'.'+ c.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') as bit) as IsIdentity" 
                + ", c.DATA_TYPE, c.NUMERIC_PRECISION, c.NUMERIC_SCALE"
                + " from INFORMATION_SCHEMA.Tables t inner join INFORMATION_SCHEMA.Columns c on t.TABLE_NAME = c.TABLE_NAME "
                + " left join (Select COLUMN_NAME, cu.TABLE_NAME, CONSTRAINT_TYPE "
                    + " from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu"
                    + " inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc"
                    + " on tc.CONSTRAINT_NAME = cu.CONSTRAINT_NAME"
                    + " WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY') x"
                + " on c.COLUMN_NAME = x.COLUMN_NAME and c.TABLE_NAME = x.TABLE_NAME"
                + " WHERE t.TABLE_TYPE = 'BASE TABLE' and t.TABLE_NAME != '__RefactorLog'"
                + " Order by c.TABLE_NAME, c.ORDINAL_POSITION";

            return GetDatabaseFields(SQL);
        }


        private List<DatabaseField> GetExternalDatabaseFields()
        {
            string SQL = "Select c.TABLE_NAME, c.COLUMN_NAME, cast(iif(x.CONSTRAINT_TYPE = 'PRIMARY KEY', 1, 0) as bit) as PrimaryKey"
                + ", cast(COLUMNPROPERTY(object_id(c.TABLE_SCHEMA +'.'+ c.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') as bit) as IsIdentity"
                + ", c.DATA_TYPE, c.NUMERIC_PRECISION, c.NUMERIC_SCALE"
                + " from INFORMATION_SCHEMA.Tables t inner join INFORMATION_SCHEMA.Columns c on t.TABLE_NAME = c.TABLE_NAME "
                + " left join sys.extended_properties ep on t.TABLE_NAME = object_Name(ep.major_Id) and c.ORDINAL_POSITION = ep.minor_id and ep.name = 'Visibility'"
                + " left join (Select COLUMN_NAME, cu.TABLE_NAME, CONSTRAINT_TYPE "
                    + " from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu"
                    + " inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc"
                    + " on tc.CONSTRAINT_NAME = cu.CONSTRAINT_NAME"
                    + " WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY') x"
                + " on c.COLUMN_NAME = x.COLUMN_NAME and c.TABLE_NAME = x.TABLE_NAME"
                + " WHERE t.TABLE_TYPE = 'BASE TABLE' and t.TABLE_NAME != '__RefactorLog'"
                + " and ep.value is null or ep.value != 'Internal'"
                + " Order by c.TABLE_NAME, c.ORDINAL_POSITION";

            return GetDatabaseFields(SQL);
        }

        private List<DatabaseField> GetDatabaseFields(String SQL)
        {
            List<DatabaseField> DatabaseFields = null;

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(SQL, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {

                    if (dr.HasRows)
                    {
                        DatabaseFields = new List<DatabaseField>();
                        while (dr.Read())
                        {
                            var newItem = new DatabaseField();
                            newItem.TableName = dr.GetString(0);
                            newItem.FieldName = dr.GetString(1);
                            newItem.PartofPrimaryKey = dr.GetBoolean(2);
                            newItem.IsIdentity = dr.GetBoolean(3);
                            newItem.SQLDataType = dr.GetString(4);
                            if (!dr.IsDBNull(5))
                            {
                                newItem.NumericPrecision = dr.GetByte(5);
                            }
                            if (!dr.IsDBNull(6))
                            {
                                newItem.NumericScale = dr.GetInt32(6);
                            }

                            DatabaseFields.Add(newItem);
                        }
                    }
                }
            }

            return DatabaseFields;
        }

        private string GetDotNetDataType(string SQLDataType)
        {
            string DotNetDataType = string.Empty;

            switch (SQLDataType)
            {
                case "bit":
                    DotNetDataType = "bool";
                    break;
                case "tinyint":
                case "smallint":
                case "int":
                    DotNetDataType = "int";
                    break;
                case "bigint":
                    DotNetDataType = "int64";
                    break;
                case "binary":
                case "varbinary":
                case "image":
                case "timestamp":
                    DotNetDataType = "byte[]";
                    break;
                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                    DotNetDataType = "string";
                    break;
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    DotNetDataType = "DateTime";
                    break;
                case "datetimeoffset":
                    DotNetDataType = "DateTimeOffset";
                    break;
                case "decimal":
                case "money":
                case "smallmoney":
                case "numeric":
                    DotNetDataType = "decimal";
                    break;
                case "float":
                    DotNetDataType = "double";
                    break;
                case "real":
                    DotNetDataType = "single";
                    break;
                case "time":
                    DotNetDataType = "TimeSpan";
                    break;
                case "uniqueidentifier":
                    DotNetDataType = "Guid";
                    break;
                case "xml":
                    DotNetDataType = "Xml";
                    break;
                default:
                    throw new Exception("Unknown SQL data type: " + SQLDataType);
            }

            return DotNetDataType;
        }

        private void AppendOverridingMethods(string Tablename, List<DatabaseField> Fields, StringBuilder sb)
        {
            sb.AppendLine("\t\tpublic void Save()");
            sb.AppendLine("\t\t{");

            sb.AppendLine("\t\t\tusing (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"BBcn\"].ConnectionString))");
            sb.AppendLine("\t\t\t{");
            sb.Append("\t\t\t\t String SQL = \"INSERT INTO ");
            sb.Append(Tablename);
            sb.Append(" (");

            var FieldList = new StringBuilder();
            foreach (var field in Fields)
            {
                FieldList.Append(field.FieldName);
                FieldList.Append(", ");
            }
            sb.Append(FieldList.ToString().Substring(0, FieldList.ToString().Length - 2));
            sb.AppendLine(")\"");
            sb.Append("\t\t\t\t\t+ \" VALUES (");
            var ValueList = new StringBuilder();
            foreach (var field in Fields)
            {
                ValueList.Append(field.FieldName);
                ValueList.Append(", ");
            }
            sb.Append(ValueList.ToString().Substring(0, ValueList.ToString().Length - 2));
            sb.AppendLine(");\";");

            sb.AppendLine("\t\t\t\tcn.Open();");

            sb.AppendLine("\t\t\t\tusing (SqlCommand cmd = new SqlCommand(SQL, cn))");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\tvar result = cmd.ExecuteNonQuery();");
            sb.AppendLine("\t\t\t\t\tif(result != 1)");
            sb.AppendLine("\t\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\t\tthrow new Exception(\"Insert of record failed\");");
            sb.AppendLine("\t\t\t\t\t}");
            if (Fields.Exists(x => x.IsIdentity == true))
            {
                sb.AppendLine("\t\t\t\t\telse");
                sb.AppendLine("\t\t\t\t\t{");
                sb.AppendLine("\t\t\t\t\tusing (SqlCommand cmd2 = new SqlCommand(\"SELECT @@IDENTITY;\"))");
                sb.AppendLine("\t\t\t\t\tusing (SqlDataReader dr = cmd2.ExecuteReader())");
                sb.AppendLine("\t\t\t\t\t\t{");
                sb.AppendLine("\t\t\t\t\t\t\twhile(dr.Read())");
                sb.AppendLine("\t\t\t\t\t\t\t{");
                sb.Append("\t\t\t\t\t\t\t\t");
                sb.Append(Fields.Single(x => x.IsIdentity == true).FieldName);
                sb.Append(" = ");
                sb.AppendLine("dr.GetDecimal(0); ");
                sb.AppendLine("\t\t\t\t\t\t\t}");
                sb.AppendLine("\t\t\t\t\t\t}");
                sb.AppendLine("\t\t\t\t\t}");
            }
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine();
            sb.AppendLine("\t\tpublic void Load(Dictionary<string, object> parms)");

            sb.AppendLine("\t\t{");

            List<DatabaseField> KeyFields = Fields.Where(x => x.PartofPrimaryKey == true).ToList<DatabaseField>();
            foreach (var KeyField in KeyFields)
            {
                string DataType = GetDotNetDataType(KeyField.SQLDataType);
                sb.Append("\t\t\t");
                sb.Append(DataType);
                sb.Append(" ");
                sb.Append(KeyField.FieldName);
                sb.Append("value = (");
                sb.Append(DataType);
                sb.Append(")parms.Where(x => x.Key == \"");
                sb.Append(KeyField.FieldName);
                sb.AppendLine("\").FirstOrDefault().Value;");
            }

            sb.AppendLine("\t\t\tusing (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings[\"DEV\"].ConnectionString))");
            sb.AppendLine("\t\t\t{");
            
            sb.Append("\t\t\t\t String SQL = \"Select ");
            sb.Append(FieldList.ToString().Substring(0, FieldList.ToString().Length - 2));
            sb.AppendLine("\"");
            sb.Append("\t\t\t\t\t+ \"FROM ");
            sb.Append(Tablename);
            sb.AppendLine("\"");
            sb.Append("\t\t\t\t\t+ \"WHERE ");

            var SelectWhereClause = new StringBuilder();
            foreach(var KeyField in KeyFields)
            {
                SelectWhereClause.Append(KeyField.FieldName);
                SelectWhereClause.Append(" = ");
                SelectWhereClause.Append(KeyField.FieldName);
                SelectWhereClause.Append("value");
                SelectWhereClause.Append(", ");
            }
            sb.Append(SelectWhereClause.ToString().Substring(0, SelectWhereClause.ToString().Length - 2));
            sb.AppendLine("\";");
            sb.AppendLine("\t\t\t\tcn.Open();");
            sb.AppendLine("\t\t\t\tusing (SqlCommand cmd = new SqlCommand(SQL, cn))");
            sb.AppendLine("\t\t\t\tusing (SqlDataReader dr = cmd.ExecuteReader())");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\tif (dr.HasRows)");
            sb.AppendLine("\t\t\t\t\t{");
            foreach(var resultField in Fields.Where(x=>x.PartofPrimaryKey == false))
            {
                sb.Append("\t\t\t\t\t\t");
                sb.Append(resultField.FieldName);
                sb.Append(" = dr.GetFieldValue<");
                sb.Append(GetDotNetDataType(resultField.SQLDataType));
                sb.Append(">(dr.GetOrdinal(\"");
                sb.Append(resultField.FieldName);
                sb.AppendLine("\"));");
            }
            sb.AppendLine("\t\t\t\t\t}");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
        }

        private class DatabaseField
        {
            public string TableName { get; set; }
            public string FieldName { get; set; }
            public bool PartofPrimaryKey { get; set; }
            public bool IsIdentity { get; set; }
            public string SQLDataType { get; set; }
            public int? NumericPrecision { get; set; }
            public int? NumericScale { get; set; }
        }

        private void GenerateDataContracts_Click(object sender, EventArgs e)
        {
            StringBuilder sb = null;
            List<DatabaseField> DatabaseFields = GetExternalDatabaseFields();
            if (DatabaseFields.Count > 0)
            {

                sb = new StringBuilder();

                sb.AppendLine("using System;");
                sb.AppendLine("using System.Runtime.Serialization;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Data;");
                sb.AppendLine("using System.Data.SqlClient;");
                sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
                sb.AppendLine();
                sb.AppendLine("namespace BB.DataContracts");
                sb.AppendLine("{");


                string CurrentTable = string.Empty;
                foreach (var Field in DatabaseFields)
                {
                    if (CurrentTable != Field.TableName)
                    {
                        if (!string.IsNullOrEmpty(CurrentTable))
                        {
                            sb.AppendLine("\t}");
                        }
                        sb.AppendLine("\t[DataContract]");
                        sb.AppendLine("\t[ExcludeFromCodeCoverage]");
                        sb.Append("\tpublic class ");
                        sb.AppendLine(Field.TableName);
                        sb.AppendLine("\t{");
                    }

                    sb.AppendLine("\t\t[DataMember]");
                    sb.Append("\t\tpublic ");
                    sb.Append(GetDotNetDataType(Field.SQLDataType));
                    sb.Append(" ");
                    sb.Append(Field.FieldName);
                    sb.AppendLine(" { get; set; }");

                    CurrentTable = Field.TableName;
                }

                sb.AppendLine("\t}");
                sb.AppendLine("}");
                string filename = Application.StartupPath + "\\..\\..\\..\\BBServices.DataContracts\\GeneratedDataTypes.cs";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (var fs = new FileStream(filename, FileMode.CreateNew))
                {
                    fs.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.ToString().Length);
                    fs.Flush();
                }
            }
        }

        private void GenerateMapping_Click(object sender, EventArgs e)
        {
            StringBuilder sb = null;
            List<DatabaseField> DatabaseFields = GetExternalDatabaseFields();
            if (DatabaseFields.Count > 0)
            {

                sb = new StringBuilder();

                sb.AppendLine("using BB.DataLayer;");
                sb.AppendLine("using BB.DataContracts;");
                sb.AppendLine("using System.Diagnostics.CodeAnalysis;");
                sb.AppendLine();
                sb.AppendLine("namespace BB.Implementation");
                sb.AppendLine("{");
                sb.AppendLine("\t[ExcludeFromCodeCoverage]");
                sb.AppendLine("\tpublic static class DLMapping");
                sb.AppendLine("\t{");

                string CurrentTable = string.Empty;
                foreach (var Field in DatabaseFields)
                {
                    
                    if (CurrentTable != Field.TableName)
                    {
                        if (!string.IsNullOrEmpty(CurrentTable))
                        {
                            sb.AppendLine("");
                            sb.AppendLine("\t\t\treturn result;");
                            sb.AppendLine("\t\t}");
                        }
                        CurrentTable = Field.TableName;

                        sb.AppendLine("");
                        sb.AppendLine("\t\t[ExcludeFromCodeCoverage]");
                        sb.Append("\t\tpublic static DL_");
                        sb.Append(Field.TableName);
                        sb.Append(" Map");
                        sb.Append(Field.TableName);
                        sb.Append("toDL");
                        sb.Append(Field.TableName);
                        sb.Append("(");
                        sb.Append(Field.TableName);
                        sb.Append(" ");
                        sb.Append(CurrentTable.ToLower());
                        sb.AppendLine(")");
                        sb.AppendLine("\t\t{");
                        sb.Append("\t\t\t");
                        sb.Append("var result = new ");
                        sb.Append("DL_");
                        sb.Append(Field.TableName);
                        sb.AppendLine("();");
                    }

                    //skip any Identity fields. These should only be set after a 
                    //save to the database
                    if (!Field.IsIdentity)
                    {
                        sb.Append("\t\t\tresult.");
                        sb.Append(Field.FieldName);
                        sb.Append(" = ");
                        sb.Append(CurrentTable.ToLower());
                        sb.Append(".");
                        sb.Append(Field.FieldName);
                        sb.AppendLine(";");
                    }

                    CurrentTable = Field.TableName;
                }

                sb.AppendLine("");
                sb.AppendLine("\t\t\treturn result;");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
                sb.AppendLine("}");

                string filename = Application.StartupPath + "\\..\\..\\..\\BBService.Implementation\\DLMapping.cs";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (var fs = new FileStream(filename, FileMode.CreateNew))
                {
                    fs.Write(Encoding.ASCII.GetBytes(sb.ToString()), 0, sb.ToString().Length);
                    fs.Flush();
                }
            }
        }
    }
}
