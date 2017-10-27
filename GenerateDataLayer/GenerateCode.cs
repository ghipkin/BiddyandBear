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
using System.Diagnostics;

namespace GenerateDataLayer
{
    enum ClassType
    {
        Abstract = 0,
        Concrete = 1,
        Mock = 3
    }

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
                sb.AppendLine("using BB.DataLayer.Abstract;");
                sb.AppendLine();
                sb.AppendLine("namespace BB.DataLayer");
                sb.AppendLine("{");

                StringBuilder AbstractClass = new StringBuilder();
                StringBuilder RecordClass= new StringBuilder();
                StringBuilder MockClass = new StringBuilder();
                string CurrentTable = string.Empty;
                List<DatabaseField> CurrentFields;
                bool IdExists;
                foreach (var Field in DatabaseFields)
                {
                    //are we onto a new table
                    if (CurrentTable != Field.TableName) 
                    {
                        //if this is not the first table, then we need to finish off the last one
                        if (!string.IsNullOrEmpty(CurrentTable))
                        {
                            CurrentFields = DatabaseFields.Where(x => x.TableName == CurrentTable).ToList<DatabaseField>();
                            IdExists = CurrentFields.Exists(x => x.IsIdentity == true);
                            FinishMockClass(MockClass, CurrentTable, IdExists);

                            //Add the overriding methods for the previous class(es)
                            FinishAbstractClass(CurrentTable, CurrentFields, AbstractClass);
                            FinishRecordClass(CurrentTable, CurrentFields, IdExists, RecordClass);

                            //append the abstract record and mocked class to the main stringbuilder
                            sb.AppendLine(AbstractClass.ToString());
                            sb.AppendLine(RecordClass.ToString());
                            sb.AppendLine(MockClass.ToString());

                            //Renew Class StringBuilders for the next table
                            AbstractClass = new StringBuilder();
                            RecordClass = new StringBuilder();
                            MockClass = new StringBuilder();
                        }
                        //Create Collection classes
                        string PluralTableName = GetPlural(Field.TableName);
                        GetParentCollectionClass(Field.TableName, PluralTableName, sb);
                        sb.AppendLine("");
                        GetCollectionClass(Field.TableName, PluralTableName, DatabaseFields.Where(x=>x.TableName==Field.TableName).ToList<DatabaseField>() , sb);
                        sb.AppendLine("");
                        GetMockCollectionClass(Field.TableName, PluralTableName, DatabaseFields.Where(x => x.TableName == Field.TableName).ToList<DatabaseField>(), sb);
                        sb.AppendLine("");                       

                        //initialise the abstract, mock and record classes
                        GetClassHeader("ADL_" + Field.TableName, AbstractClass);
                        GetClassHeader("DL_" + Field.TableName, RecordClass, "ADL_" + Field.TableName);
                        GetClassHeader("MOCK_DL_" + Field.TableName, MockClass, "ADL_" + Field.TableName);
                        //add Error message constant to the mock class
                        MockClass.Append("\t\tpublic const string ERR_SAVE_FAILED = \"Could not save ");
                        MockClass.Append(Field.TableName);
                        MockClass.AppendLine(" record.\";");

                    }

                    //get the property code for the abstract class
                    GetPropertyText(Field, AbstractClass);

                    //get the current table
                    CurrentTable = Field.TableName;
                }

                CurrentFields = DatabaseFields.Where(x => x.TableName == CurrentTable).ToList<DatabaseField>();
                IdExists = CurrentFields.Exists(x => x.IsIdentity == true);

                FinishMockClass(MockClass, CurrentTable, IdExists);

                //Add the overriding methods for the previous class(es)
                FinishAbstractClass(CurrentTable, CurrentFields, AbstractClass);
                FinishRecordClass(CurrentTable, CurrentFields, IdExists, RecordClass);

                //append the record and mocked class to the main stringbuilder
                sb.AppendLine(AbstractClass.ToString());
                sb.AppendLine();
                sb.AppendLine(RecordClass.ToString());
                sb.AppendLine();
                sb.AppendLine(MockClass.ToString());

                sb.AppendLine("}");

                string filename = Application.StartupPath + "\\..\\..\\..\\BB.DataLayer\\BBDataLayer.cs";
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

        private void FinishMockClass(StringBuilder MockClass, String Table, bool IdExists)
        {
            //add extra properties for the MOCK class
            MockClass.AppendLine("\t\tpublic bool fail { get; set; }");
            if (IdExists)
            {
                MockClass.Append("\t\tpublic const long DEFAULT_");
                MockClass.Append(Table.ToUpper());
                MockClass.Append("_ID = ");
                //seems the nearest way to generate a unique ID for these is to use a random number
                var r = new System.Random();
                MockClass.Append(r.Next(int.MaxValue).ToString());
                MockClass.AppendLine(";");

                AppendMockedMethods(MockClass, Table,IdExists);
            }
            else
            {
                AppendMockedMethods(MockClass);
            }

            //close the class
            MockClass.AppendLine("\t}");
        }

        private string GetFieldList(List<DatabaseField> DatabaseFields)
        {
            //Get the list of fields in a string for this table
            var sbFieldList = new StringBuilder();
            foreach (var field in DatabaseFields)
            {
                sbFieldList.Append(field.FieldName);
                sbFieldList.Append(", ");
            }
            //Remove the last two characters(i.e. ", ")
            string FieldList = sbFieldList.ToString().Substring(0, sbFieldList.ToString().Length - 2);

            return FieldList;
        }

        private void GetClassHeader(String ClassName, StringBuilder sb, string ParentClassName = null)
        {
            sb.AppendLine("\t[ExcludeFromCodeCoverage]");
            sb.Append("\tpublic ");
            if(ParentClassName == null)//Then this must be the parent class
            {
                sb.Append("abstract ");
            }

            sb.Append("class ");
            sb.Append(ClassName);
            if (ParentClassName == null)
            {
                sb.AppendLine(" : DatabaseRecord");
            }
            else
            {
                sb.Append(" : ");
                sb.AppendLine(ParentClassName);
            }
            sb.AppendLine("\t{");
        }

        private void GetPropertyText(DatabaseField Field, StringBuilder sb)
        {
            sb.Append("\t\tpublic ");
            sb.Append(GetDotNetDataType(Field.SQLDataType, Field.NumericScale));
            sb.Append(" ");
            sb.Append(Field.FieldName);
            sb.Append(" { get;");
            if (Field.SQLDataType == "Timestamp") //should not be able to change timestamp fields
            {
                sb.Append(" private");
            }
            else if (Field.IsIdentity)//should only be able to set identity fields inside the Datalayer dll
            {
                sb.Append(" internal");
            }
            sb.Append(" set; }");
            sb.AppendLine();
        }

        private void GetParentCollectionClass(string TableName, string PluralName, StringBuilder sb)
        {

            sb.AppendLine("\t[ExcludeFromCodeCoverage]");
            sb.Append("\tpublic abstract class ");
            sb.Append("ADL_");
            sb.Append(PluralName);
            sb.AppendLine(" : DatabaseRecords");
            sb.AppendLine("\t{");
            //Add the constructor
            sb.Append("\t\tpublic List <ADL_");
            sb.Append(TableName);
            sb.Append("> ");
            sb.Append(PluralName);
            sb.AppendLine(" { get; protected set; }");
            sb.AppendLine("\t}");
            sb.AppendLine("");
        }

        private void GetCollectionClass(string TableName, string PluralName, List<DatabaseField> Fields, StringBuilder sb)
        {
            sb.AppendLine("\t[ExcludeFromCodeCoverage]");
            sb.Append("\tpublic class ");
            sb.Append("DL_");
            sb.Append(PluralName);
            sb.Append(" : ADL_");
            sb.AppendLine(PluralName);
            
            sb.AppendLine("\t{");
            //Add the constructor
            sb.Append("\t\tpublic DL_");
            sb.Append(PluralName);
            sb.AppendLine("()");
            sb.AppendLine("\t\t{");
            sb.Append("\t\t\t");
            sb.Append(PluralName);
            sb.Append(" = new List<ADL_");
            sb.Append(TableName);
            sb.AppendLine(">();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");

            GetLoadRecordsMethod(TableName, PluralName, Fields, sb);
            sb.AppendLine();
            GetSaveRecordsMethod(TableName, PluralName, Fields, sb);

            sb.AppendLine("\t}");
            sb.AppendLine("");
        }

        private string GetPlural(string name)
        {
            string plural;
            if (name.EndsWith("y"))
            {
                plural = name.Substring(0,name.Length - 1) + "ies";
            }
            else
            {
                plural = name + "s";
            }

            return plural;
        }

        private void GetMockCollectionClass(string TableName, string PluralName, List<DatabaseField> Fields, StringBuilder sb)
        {
            sb.AppendLine("\t[ExcludeFromCodeCoverage]");
            sb.Append("\tpublic class ");
            sb.Append("MOCK_DL_");
            sb.Append(PluralName);
            sb.Append(" : ADL_");
            sb.AppendLine(PluralName);
            sb.AppendLine("\t{");
            sb.Append("\t\tpublic const string ERR_FAILURE = \"Failed to save or load ");
            sb.Append(PluralName);
            sb.AppendLine(" records\";");
            sb.AppendLine("");
            //Add the Failure Property
            sb.AppendLine("\t\tpublic bool Failure{ get; set; }");
            //Add the constructor
            sb.Append("\t\tpublic MOCK_DL_");
            sb.Append(PluralName);
            sb.AppendLine("()");
            sb.AppendLine("\t\t{");
            sb.Append("\t\t\t");
            sb.Append(PluralName);
            sb.Append(" = new List<ADL_");
            sb.Append(TableName);
            sb.AppendLine(">();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");

            //GetLoadRecordsMethod(TableName, PluralName, Fields, sb);
            sb.AppendLine("\t\tpublic override void LoadRecords(Dictionary<String, Object> WhereParams)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif(Failure)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tthrow new Exception(ERR_FAILURE);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tforeach(var record in BB.Mocks.MockDatabase.MockedDb)");
            sb.AppendLine("\t\t\t{");
            sb.Append("\t\t\t\t");
            sb.Append(PluralName);
            sb.Append(".Add((ADL_");
            sb.Append(TableName);
            sb.AppendLine(")record);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine();

            //Save Method
            sb.AppendLine("\t\tpublic override void SaveRecords(SqlConnection cn = null)");
            sb.AppendLine("\t\t{");
            sb.Append("\t\t\tforeach(var record in ");
            sb.Append(PluralName);
            sb.AppendLine(")");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tif(Failure)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\tthrow new Exception(ERR_FAILURE);");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t\tBB.Mocks.MockDatabase.Insert(record);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");

            sb.AppendLine("\t}");
            sb.AppendLine("");
        }

        private void GetLoadRecordsMethod(string TableName, string PluralName, List<DatabaseField> Fields, StringBuilder sb)
        {
            sb.AppendLine("\t\tpublic override void LoadRecords(Dictionary<String, Object> WhereParams)");
            sb.AppendLine("\t\t{");
            sb.Append("\t\t\tList<ADL_");
            sb.Append(TableName);
            sb.AppendLine("> result;");
            sb.AppendLine("\t\t\tstring SQL;");
            sb.Append("\t\t\tvar FirstSQL = \"Select ");
            sb.Append(GetFieldList(Fields));
            sb.AppendLine("\"");
            sb.Append("\t\t\t+ \" FROM ");
            sb.Append(TableName);
            sb.AppendLine("\";");
            sb.AppendLine("\t\t\tif(WhereParams != null || WhereParams.Count == 0)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tvar sbSQL = new StringBuilder();");
            sb.AppendLine("\t\t\t\tsbSQL.Append(FirstSQL);");
            sb.AppendLine("\t\t\t\tsbSQL.Append(\" WHERE \");");
            sb.AppendLine("\t\t\t\tforeach(var param in WhereParams)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine("\t\t\t\t\tsbSQL.AppendLine(param.Key);");
            sb.AppendLine("\t\t\t\t\tsbSQL.Append(\" = \");");
            sb.AppendLine("\t\t\t\t\tsbSQL.Append(param.Value);");
            sb.AppendLine("\t\t\t\t\tsbSQL.AppendLine(\" AND\");");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t\tsbSQL.Remove(sbSQL.Length-2,2);");
            sb.AppendLine("\t\t\t\tSQL = sbSQL.ToString();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tSQL = FirstSQL;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tusing (SqlConnection cn = GetSQLConnection())");
            sb.AppendLine("\t\t\tusing (SqlCommand cmd = new SqlCommand(SQL))");
            sb.AppendLine("\t\t\tusing (SqlDataReader dr = cmd.ExecuteReader())");
            sb.AppendLine("\t\t\t{");
            sb.Append("\t\t\t\tresult = new List<ADL_");
            sb.Append(TableName);
            sb.AppendLine(">();");
            sb.AppendLine("\t\t\t\twhile(dr.Read())");
            sb.AppendLine("\t\t\t\t{");
            sb.Append("\t\t\t\t\tvar NewRow = new DL_");
            sb.Append(TableName);
            sb.AppendLine("();");
            foreach (var field in Fields)
            {
                sb.Append("\t\t\t\t\tNewRow.");
                sb.Append(field.FieldName);
                sb.Append(" = dr.GetFieldValue<");
                sb.Append(GetDotNetDataType(field.SQLDataType, field.NumericScale));
                sb.Append(">(dr.GetOrdinal(\"");
                sb.Append(field.FieldName);
                sb.AppendLine("\"));");
            }
            sb.AppendLine("\t\t\t\t\tresult.Add(NewRow);");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.Append("\t\t\t");
            sb.Append(PluralName);
            sb.AppendLine(" = result;");
            sb.AppendLine("\t\t}");
        }

        private void GetSaveRecordsMethod(string TableName, string PluralName, List<DatabaseField> Fields, StringBuilder sb)
        {
            sb.AppendLine("\t\tpublic override void SaveRecords(SqlConnection cn = null)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tbool NeedtoCloseConnection = false;");
            sb.AppendLine("");
            sb.AppendLine("\t\t\tvar InsertSQL = new StringBuilder();");
            sb.Append("\t\t\tforeach(var record in ");
            sb.Append(PluralName);
            sb.AppendLine(")");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tInsertSQL.Append(record.GetInsertSQL());");
            sb.AppendLine("\t\t\t\tInsertSQL.Append(\";\");");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\t\tif(cn == null)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tcn = GetSQLConnection();");
            sb.AppendLine("\t\t\t\tNeedtoCloseConnection = true;");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\t\tusing (SqlCommand cmd = new SqlCommand(InsertSQL.ToString().Substring(0, InsertSQL.ToString().Length -4)))");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tvar SaveResult = cmd.ExecuteNonQuery();");
            sb.AppendLine("\t\t\t\tif(SaveResult < 1)");
            sb.AppendLine("\t\t\t\t{");
            sb.Append("\t\t\t\t\tthrow new Exception(\"Save of ");
            sb.Append(TableName);
            sb.AppendLine(" records failed.\");");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\t\tif(NeedtoCloseConnection)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tcn.Close();");
            sb.AppendLine("\t\t\t\tcn.Dispose();");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
        }

        private List<DatabaseField> GetAllDatabaseFields()
        {
            string SQL = "Select c.TABLE_NAME, t.TABLE_TYPE, c.COLUMN_NAME, cast(iif(x.CONSTRAINT_TYPE = 'PRIMARY KEY', 1, 0) as bit) as PrimaryKey"
                + ", cast(COLUMNPROPERTY(object_id(c.TABLE_SCHEMA +'.'+ c.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') as bit) as IsIdentity" 
                + ", c.DATA_TYPE, c.NUMERIC_PRECISION, c.NUMERIC_SCALE"
                + " from INFORMATION_SCHEMA.Tables t inner join INFORMATION_SCHEMA.Columns c on t.TABLE_NAME = c.TABLE_NAME "
                + " left join sys.extended_properties ep on t.TABLE_NAME = object_Name(ep.major_Id) and ep.minor_id = 0 "
                + " left join (Select COLUMN_NAME, cu.TABLE_NAME, CONSTRAINT_TYPE "
                    + " from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu"
                    + " inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc"
                    + " on tc.CONSTRAINT_NAME = cu.CONSTRAINT_NAME"
                    + " WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY') x"
                + " on c.COLUMN_NAME = x.COLUMN_NAME and ((c.TABLE_NAME = x.TABLE_NAME and t.TABLE_TYPE = 'BASE TABLE')"
                    + " OR x.TABLE_NAME = (Select TABLE_NAME"
                                   + " FROM INFORMATION_SCHEMA.VIEW_TABLE_USAGE"
                                   + " Where VIEW_NAME = c.TABLE_NAME and t.TABLE_TYPE = 'VIEW'))"
                + " WHERE t.TABLE_TYPE in ('BASE TABLE', 'VIEW') and ep.name is null or ep.name != 'microsoft_database_tools_support'"
                + " Order by c.TABLE_NAME, c.ORDINAL_POSITION";

            return GetDatabaseFields(SQL);
        }


        private List<DatabaseField> GetExternalDatabaseFields()
        {
            string SQL = "Select c.TABLE_NAME, t.TABLE_TYPE, c.COLUMN_NAME, cast(iif(x.CONSTRAINT_TYPE = 'PRIMARY KEY', 1, 0) as bit) as PrimaryKey"
                + ", cast(COLUMNPROPERTY(object_id(c.TABLE_SCHEMA +'.'+ c.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') as bit) as IsIdentity"
                + ", c.DATA_TYPE, c.NUMERIC_PRECISION, c.NUMERIC_SCALE"
                + " from INFORMATION_SCHEMA.Tables t inner join INFORMATION_SCHEMA.Columns c on t.TABLE_NAME = c.TABLE_NAME "
                + " left join sys.extended_properties ep1 on t.TABLE_NAME = object_Name(ep1.major_Id) and ep1.minor_id = 0 "
                + " left join sys.extended_properties ep2 on t.TABLE_NAME = object_Name(ep2.major_Id) and c.ORDINAL_POSITION = ep2.minor_id and ep2.name = 'Visibility'"
                + " left join (Select COLUMN_NAME, cu.TABLE_NAME, CONSTRAINT_TYPE "
                    + " from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cu"
                    + " inner join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc"
                    + " on tc.CONSTRAINT_NAME = cu.CONSTRAINT_NAME"
                    + " WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY') x"
                + " on c.COLUMN_NAME = x.COLUMN_NAME and ((c.TABLE_NAME = x.TABLE_NAME and t.TABLE_TYPE='BASE TABLE')"
                                    + " or x.TABLE_NAME = (Select TABLE_NAME" 
                                                        + " FROM INFORMATION_SCHEMA.VIEW_TABLE_USAGE"
                                                        + " Where VIEW_NAME = c.TABLE_NAME and t.TABLE_TYPE = 'VIEW') )"
                + " WHERE t.TABLE_TYPE in ('BASE TABLE', 'VIEW') and t.TABLE_NAME != '__RefactorLog'"
                + " and ep1.name is null or ep1.name != 'microsoft_database_tools_support'"
                + " and ep2.value is null or ep2.value != 'Internal'"
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
                            newItem.TableType = dr.GetString(1);
                            newItem.FieldName = dr.GetString(2);
                            newItem.PartofPrimaryKey = dr.GetBoolean(3);
                            newItem.IsIdentity = dr.GetBoolean(4);
                            newItem.SQLDataType = dr.GetString(5);
                            if (!dr.IsDBNull(6))
                            {
                                newItem.NumericPrecision = dr.GetByte(6);
                            }
                            if (!dr.IsDBNull(7))
                            {
                                newItem.NumericScale = dr.GetInt32(7);
                            }

                            DatabaseFields.Add(newItem);
                        }
                    }
                }
            }

            return DatabaseFields;
        }

        private string GetDotNetDataType(string SQLDataType, int? scale)
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
                    DotNetDataType = "decimal";
                    break;
                case "numeric":
                    DotNetDataType = DecodeNumeric((int)scale);
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

        private string DecodeNumeric(int scale)
        {
            string datatype = string.Empty;
            if(scale == 0)
            {
                datatype = "long";
            }
            else
            {
                datatype = "decimal";
            }
            return datatype;
        }

        private void FinishAbstractClass(String TableName, List<DatabaseField> Fields, StringBuilder sb)
        {
            GetInsertSQL(TableName, GetFieldList(Fields), sb);
            sb.AppendLine();
            sb.AppendLine("\t}");
        }

        private void GetInsertSQL(string Tablename, string FieldList, StringBuilder sb)
        {
            sb.AppendLine("\t\tpublic override string GetInsertSQL()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif(String.IsNullOrEmpty(InsertSQL))");
            sb.AppendLine("\t\t\t{");
            sb.Append("\t\t\t\tInsertSQL = \"INSERT INTO ");
            sb.Append(Tablename);
            sb.Append(" (");
            sb.Append(FieldList);
            sb.AppendLine(")\"");
            sb.Append("\t\t\t\t\t+ \" VALUES (");
            var ValueList = new StringBuilder();
            foreach (var field in FieldList.Split(','))
            {
                ValueList.Append("\" + this.");
                ValueList.Append(field.Trim());
                ValueList.Append(" + \"");
                ValueList.Append(", ");
            }
            sb.Append(ValueList.ToString().Substring(0, ValueList.ToString().Length - 2));
            sb.AppendLine(");\";");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn InsertSQL;");
            sb.Append("\t\t}");
            sb.AppendLine();
        }

        private void FinishRecordClass(string Tablename, List<DatabaseField> Fields, bool IdExists, StringBuilder sb)
        {
            //check is we are a view or not
            string TableType = Fields.Select(x => x.TableType).Distinct().SingleOrDefault();
            string FieldList = GetFieldList(Fields);

            GetInsertSQL(Tablename, FieldList, sb);

            sb.AppendLine("\t\tpublic override void Save(SqlConnection cn = null)");
            sb.AppendLine("\t\t{");
            //Do not flesh out the Save method for Views
            if (TableType != "VIEW")
            {
                
                sb.AppendLine("\t\t\tvar SQL = GetInsertSQL();");//Get the INSERT statement
                sb.AppendLine("\t\t\tbool NeedtoCloseConnection = false;");
                sb.AppendLine("\t\t\tif(cn == null)");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine("\t\t\t\tcn = GetSQLConnection();");
                sb.AppendLine("\t\t\t\tcn.Open();");
                sb.AppendLine("\t\t\t\tNeedtoCloseConnection = true;");
                sb.AppendLine("\t\t\t}");
                sb.AppendLine("\t\t\tusing (SqlCommand cmd = new SqlCommand(SQL, cn))");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine("\t\t\t\tvar result = cmd.ExecuteNonQuery();");//Save the record
                sb.AppendLine("\t\t\t\tif(result != 1)");
                sb.AppendLine("\t\t\t\t{");
                sb.Append("\t\t\t\t\tthrow new Exception(\"Insert of ");
                sb.Append(Tablename);
                sb.AppendLine(" record failed\");");
                sb.AppendLine("\t\t\t\t}");
                if (IdExists)//Now get the identity field of the record just saved
                {
                    sb.AppendLine("\t\t\t\telse");
                    sb.AppendLine("\t\t\t\t{");
                    sb.AppendLine("\t\t\t\tusing (SqlCommand cmd2 = new SqlCommand(\"SELECT @@IDENTITY;\"))");
                    sb.AppendLine("\t\t\t\tusing (SqlDataReader dr = cmd2.ExecuteReader())");
                    sb.AppendLine("\t\t\t\t\t{");
                    sb.AppendLine("\t\t\t\t\t\twhile(dr.Read())");
                    sb.AppendLine("\t\t\t\t\t\t{");
                    sb.Append("\t\t\t\t\t\t\t");
                    sb.Append(Fields.Where(x => x.IsIdentity == true).SingleOrDefault().FieldName);
                    sb.Append(" = ");
                    sb.AppendLine("dr.GetInt64(0); ");
                    sb.AppendLine("\t\t\t\t\t\t}");
                    sb.AppendLine("\t\t\t\t\t}");
                    sb.AppendLine("\t\t\t\t}");
                }
                sb.AppendLine("\t\t\t}");
                sb.AppendLine("\t\t\tif(NeedtoCloseConnection);");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine("\t\t\t\tcn.Close();");
                sb.AppendLine("\t\t\t\tcn.Dispose();");
                sb.AppendLine("\t\t\t}");
            }
            sb.AppendLine("\t\t}");
            sb.AppendLine();
            
            sb.AppendLine("\t\tpublic override void Load(Dictionary<string, object> parms)");
            sb.AppendLine("\t\t{");

            //In theory it is possible to have a view which has no primary key. However this Load method is designed only to load one record,
            //so here we assume that we are selecting by the primary key
            List<DatabaseField> KeyFields = Fields.Where(x => x.PartofPrimaryKey == true).ToList<DatabaseField>();
            foreach (var KeyField in KeyFields)
            {
                string DataType = GetDotNetDataType(KeyField.SQLDataType, KeyField.NumericScale);
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

            sb.AppendLine("\t\t\tusing (SqlConnection cn = GetSQLConnection())");
            sb.AppendLine("\t\t\t{");
            
            sb.Append("\t\t\t\t String SQL = \"Select ");
            sb.Append(FieldList);
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
                sb.Append(GetDotNetDataType(resultField.SQLDataType, resultField.NumericScale));
                sb.Append(">(dr.GetOrdinal(\"");
                sb.Append(resultField.FieldName);
                sb.AppendLine("\"));");
            }
            sb.AppendLine("\t\t\t\t\t}");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            //close the class
            sb.AppendLine("\t}");
        }

        private void AppendMockedMethods(StringBuilder sb, string table = null, bool IdExists = false)
        {
            sb.AppendLine("\t\tpublic override string GetInsertSQL()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\treturn String.Empty;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");
            sb.AppendLine("\t\tpublic override void Save(SqlConnection cn = null)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif(fail)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tthrow new Exception(ERR_SAVE_FAILED);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine("\t\t\t{");

            if (IdExists)
            {
                sb.Append("\t\t\t\tthis.Id = ");
                sb.Append("DEFAULT_");
                sb.Append(table.ToUpper());
                sb.AppendLine("_ID;");
            }
            
            sb.AppendLine("\t\t\t\tBB.Mocks.MockDatabase.Insert(this);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine();
            sb.AppendLine("\t\tpublic override void Load(Dictionary<string, object> parms)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t}");
        }

        private class DatabaseField
        {
            public string TableName { get; set; }
            public string TableType { get; set; }
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
                    sb.Append(GetDotNetDataType(Field.SQLDataType, Field.NumericScale));
                    sb.Append(" ");
                    sb.Append(Field.FieldName);
                    sb.AppendLine(" { get; set; }");

                    CurrentTable = Field.TableName;
                }

                sb.AppendLine("\t}");
                sb.AppendLine("}");
                string filename = Application.StartupPath + "\\..\\..\\..\\BB.DataContracts\\GeneratedDataTypes.cs";
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
                AppendAbstractMappingClass(DatabaseFields.Select(x=>x.TableName).Distinct<String>(),sb);
                sb.AppendLine("");
                AppendMappingClass(DatabaseFields, sb, ClassType.Concrete);
                sb.AppendLine("");
                AppendMappingClass(DatabaseFields, sb, ClassType.Mock);
                //close the Namespace
                sb.Append("}");

                string filename = Application.StartupPath + "\\..\\..\\..\\BB.Implementation\\DLMapping.cs";
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

        private void AppendAbstractMappingClass(IEnumerable<string> Tables, StringBuilder sb)
        {
            sb.AppendLine("\t[ExcludeFromCodeCoverage]");
            sb.AppendLine("\tpublic abstract class ADL_Mapping");
            sb.AppendLine("\t{");
            foreach (string table in Tables)
            {
                GetMappingFromDLMethodHeader(table, sb, true);
                sb.AppendLine("");
                GetMappingToDLMethodHeader(table, sb, ClassType.Abstract);
                sb.AppendLine("");
            }
            sb.AppendLine("\t}");
        }

        private void AppendMappingClass(IEnumerable<DatabaseField> Fields, StringBuilder sb, ClassType classtype)
        {
            sb.AppendLine("\t[ExcludeFromCodeCoverage]");
            sb.Append("\tpublic class ");
            if (classtype == ClassType.Mock)
            {
                sb.Append("Mock_");
            }
            sb.Append("DLMapping : ADL_Mapping");
            sb.AppendLine("\t{");

            var ToDL = new StringBuilder();
            var FromDL = new StringBuilder();
            string CurrentTable = string.Empty;
            foreach (var Field in Fields)
            {
                if (CurrentTable != Field.TableName)
                {
                    //If we are onto a new table, then we should finish off the last one
                    if (!string.IsNullOrEmpty(CurrentTable))
                    {
                        ToDL.AppendLine("");
                        ToDL.AppendLine("\t\t\treturn result;");
                        ToDL.AppendLine("\t\t}");

                        FromDL.AppendLine("");
                        FromDL.AppendLine("\t\t\treturn result;");
                        FromDL.AppendLine("\t\t}");

                        //write each class to the main string builder
                        sb.Append(FromDL.ToString());
                        sb.Append(ToDL.ToString());

                        ToDL = new StringBuilder();
                        FromDL = new StringBuilder();
                    }

                    GetMappingFromDLMethodHeader(Field.TableName, FromDL);
                    GetMappingToDLMethodHeader(Field.TableName, ToDL, classtype);

                    CurrentTable = Field.TableName;
                }

                //Skip any Identity fields when mapping to a DL.
                // These should only be set after a save to the database
                if (!Field.IsIdentity)
                {
                    GetMappingPropertyText(Field.FieldName, CurrentTable, ToDL);
                }
                GetMappingPropertyText(Field.FieldName, CurrentTable, FromDL);
                

                CurrentTable = Field.TableName;
            }

            ToDL.AppendLine("");
            ToDL.AppendLine("\t\t\treturn result;");
            ToDL.AppendLine("\t\t}");

            FromDL.AppendLine("");
            FromDL.AppendLine("\t\t\treturn result;");
            FromDL.AppendLine("\t\t}");

            //write the last classes to the main string builder
            sb.Append(FromDL.ToString());
            sb.Append(ToDL.ToString());

            //close the class
            sb.Append("\t}");
        }

        private void GetMappingToDLMethodHeader(string TableName, StringBuilder sb, ClassType classtype)
        {
            sb.AppendLine("");
            sb.AppendLine("\t\t[ExcludeFromCodeCoverage]");
            sb.Append("\t\tpublic ");
            if(classtype == ClassType.Abstract)
            {
                sb.Append("abstract ");
            }
            else
            {
                sb.Append("override ");
            }

            sb.Append("ADL_");
            sb.Append(TableName);
            sb.Append(" Map");
            sb.Append(TableName);
            sb.Append("toDL");
            sb.Append(TableName);
            sb.Append("(");
            sb.Append(TableName);
            sb.Append(" ");
            sb.Append(TableName.ToLower());
            if (classtype == ClassType.Abstract)
            {
                sb.AppendLine(");");
            }
            else
            {
                sb.AppendLine(")");
                sb.AppendLine("\t\t{");
                sb.Append("\t\t\t");
                sb.Append("var result = new ");
                if(classtype == ClassType.Mock)
                {
                    sb.Append("MOCK_");
                }
                sb.Append("DL_");
                sb.Append(TableName);
                sb.AppendLine("();");
            }
        }

        private void GetMappingFromDLMethodHeader(string TableName, StringBuilder sb, bool Abstract = false)
        {
            sb.AppendLine("");
            sb.AppendLine("\t\t[ExcludeFromCodeCoverage]");
            sb.Append("\t\tpublic ");
            if (Abstract)
            {
                sb.Append("abstract ");
            }
            else
            {
                sb.Append("override ");
            }
            sb.Append(TableName);
            sb.Append(" Map");
            sb.Append(TableName);
            sb.Append("fromDL");
            sb.Append(TableName);
            sb.Append("(ADL_");
            sb.Append(TableName);
            sb.Append(" ");
            sb.Append(TableName.ToLower());
            if (Abstract)
            {
                sb.AppendLine(");");
            }
            else
            {
                sb.AppendLine(")");
                sb.AppendLine("\t\t{");
                sb.Append("\t\t\t");
                sb.Append("var result = new ");
                sb.Append(TableName);
                sb.AppendLine("();");
            }
        }

        private void GetMappingPropertyText(String FieldName, string TableName, StringBuilder sb)
        {
            sb.Append("\t\t\tresult.");
            sb.Append(FieldName);
            sb.Append(" = ");
            sb.Append(TableName.ToLower());
            sb.Append(".");
            sb.Append(FieldName);
            sb.AppendLine(";");
        }

        private void lblWarning2_Click(object sender, EventArgs e)
        {

        }
    }
}
