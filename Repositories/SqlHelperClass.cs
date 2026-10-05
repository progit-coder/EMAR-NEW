using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{

    public class SqlHelperClass
    {
        public List<string> ParameterName = new List<string>();
        public List<object> ParamValue = new List<object>();
        public List<SqlDbType> ParameterType = new List<SqlDbType>();
        public List<int> ParameterSize = new List<int>();

        /// <summary>
        /// Parameter Passing and values.
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="ParameterType"></param>
        /// <param name="ParamValue"></param>
        /// <param name="ParameterSize"></param>
        public void AddParameter(string ParameterName, SqlDbType ParameterType, object ParamValue, int ParameterSize)
        {
            this.ParameterName.Add(ParameterName);
            this.ParameterType.Add(ParameterType);
            this.ParamValue.Add(ParamValue);
            this.ParameterSize.Add(ParameterSize);

        }

        /// <summary>
        /// Parameter Passing And values.
        /// </summary>
        /// <param name="ParameterName"></param>
        /// <param name="ParameterType"></param>
        /// <param name="ParamValue"></param>
        public void AddParameter(string ParameterName, SqlDbType ParameterType, object ParamValue)
        {
            this.ParameterName.Add(ParameterName);
            this.ParameterType.Add(ParameterType);
            this.ParamValue.Add(ParamValue);

        }
        /// <summary>
        ///  passing itemIndex
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>

        public SqlParameter GetParameter(int itemIndex)
        {
            SqlParameter sp = new SqlParameter(this.ParameterName[itemIndex], this.ParameterType[itemIndex]);
            sp.Value = this.ParamValue[itemIndex];
            switch (this.ParameterType[itemIndex])
            {
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                    if ((this.ParamValue[itemIndex] == null))
                    {
                        sp.Size = 0;
                    }
                    else
                    {
                        sp.Size = this.ParamValue[itemIndex].ToString().Length;
                    }
                    break;
                default:
                    break;
            }

            return sp;
        }
    }

    public class SQLHelper
    {
        private readonly string _connectionString;
        private readonly string _pgconnectionString;

        //private static SQLHelper _instance;
        //private readonly bool _debugEnabled;


        //public SQLHelper(string connectionString)
        public SQLHelper()
        {
            //_connectionString = connectionString;
            try
            {
                //_connectionString = ConfigurationManager.ConnectionStrings["RevvProConnectionString"].ToString(); //ConfigurationManager.AppSettings["RevvProConnectionString"];
                _connectionString = "EMARReportsEntities";
                _pgconnectionString = "EMAREntities";
            }
            catch
            {
            }
        }

        //public static SQLHelper GetInstance()
        //{
        //    if ((_instance == null))
        //        _instance = new SQLHelper("RevvProConnectionString");

        //    return _instance;
        //}

        public DataTable ExecuteStoredProcedure(string procedureName)
        {
            SqlParameter[] Params = null;
            return ExecuteStoredProcedure(procedureName, Params);
        }



        public DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters)
        {
            var dt = new DataTable();

            var conn = OpenDbConnection();
            //Only try to execute the stored procedure if connected to the database
            if ((conn.State == ConnectionState.Open))
            {
                try
                {
                    //stored procedure
                    var dataAdapter = new SqlDataAdapter(procedureName, conn) { SelectCommand = { CommandType = CommandType.StoredProcedure } };
                    //stored procedure parameters
                    if ((parameters != null))
                    {
                        foreach (var param in parameters)
                        {
                            dataAdapter.SelectCommand.Parameters.Add(param);
                            dataAdapter.SelectCommand.CommandTimeout = 180;
                        }
                    }
                    //use datatable as equiv of a recordset:
                    dataAdapter.Fill(dt);
                }
                catch (SqlException ex)
                {

                    throw;

                }
                finally
                {
                    CloseDbConnection(conn);
                }
            }


            //return DataTable
            return dt;
        }

        public DataTable ExecuteStoredProcedure(string procedureName, SqlHelperClass Parameters)
        {
            var dt = new DataTable();
            var conn = OpenDbConnection();
            //Only try to execute the stored procedure if connected to the database
            if ((conn.State == ConnectionState.Open))
            {
                try
                {
                    var dataAdapter = new SqlDataAdapter(procedureName, conn) { SelectCommand = { CommandType = CommandType.StoredProcedure } };
                    if ((Parameters != null))
                    {
                        for (int I = 0; I <= (Parameters.ParameterName.Count() - 1); I++)
                        {
                            SqlParameter param = Parameters.GetParameter(I);
                            dataAdapter.SelectCommand.Parameters.Add(param);
                            dataAdapter.SelectCommand.CommandTimeout = 180;
                        }
                    }
                    //use datatable as equiv of a recordset:
                    dataAdapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    throw;

                }
                finally
                {
                    CloseDbConnection(conn);
                }
            }
            return dt;
        }

        public DataSet ExecuteStoredProcedureReturnDataSet(string procedureName, SqlParameter[] parameters)
        {
            var ds = new DataSet();
            var conn = OpenDbConnection();

            if ((conn.State == ConnectionState.Open))
            {
                try
                {
                    //stored procedure
                    var dataAdapter = new SqlDataAdapter(procedureName, conn) { SelectCommand = { CommandType = CommandType.StoredProcedure } };
                    //stored procedure parameters
                    if ((parameters != null))
                    {
                        foreach (var param in parameters)
                        {
                            dataAdapter.SelectCommand.Parameters.Add(param);
                            dataAdapter.SelectCommand.CommandTimeout = 180;
                        }
                    }
                    //use datatable as equiv of a recordset:
                    dataAdapter.Fill(ds);
                }
                catch (SqlException ex)
                {
                    throw;

                }
                finally
                {
                    CloseDbConnection(conn);
                }
            }


            return ds;
        }

        public DataTable ExecuteStoredProcedureReturnDataTable(string procedureName, SqlParameter[] parameters)
        {
            var dt = new DataTable();
            var conn = OpenDbConnection();


            //Only try to execute the stored procedure if connected to the database
            if ((conn.State == ConnectionState.Open))
            {
                try
                {
                    //stored procedure
                    var dataAdapter = new SqlDataAdapter(procedureName, conn) { SelectCommand = { CommandType = CommandType.StoredProcedure } };
                    //stored procedure parameters
                    if ((parameters != null))
                    {
                        foreach (var param in parameters)
                        {
                            dataAdapter.SelectCommand.Parameters.Add(param);
                            dataAdapter.SelectCommand.CommandTimeout = 3600;
                        }
                    }
                    //use datatable as equiv of a recordset:
                    dataAdapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    throw;

                }
                finally
                {
                    CloseDbConnection(conn);
                }
            }

            return dt;
        }

        public DataTable ExecutePGCommand(string Query)
        {
            //DataTable dt = new DataTable();
            //NpgsqlConnection pgcon = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[_pgconnectionString].ConnectionString);
            //NpgsqlCommand pgcom = new NpgsqlCommand();
            //pgcom.Connection = pgcon;

            //using (pgcon)
            //{
            //    try
            //    {
            //        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter())
            //        {
            //            pgcom.CommandText = Query;
            //            da.SelectCommand = pgcom;
            //            da.SelectCommand.CommandType = CommandType.Text;
            //            da.Fill(dt);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error: " + e.Message);
            //    }
            //}

            return null;
        }
        private SqlConnection OpenDbConnection()
        {
            SqlConnection conn;
            try
            {
                //Modify to your settings:
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionString].ConnectionString);
                conn.Open();
            }

            catch (Exception ex)
            {
                throw;
            }
            return conn;
        }

        private static void CloseDbConnection(IDbConnection conn)
        {
            try
            {
                conn.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
