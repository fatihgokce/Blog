using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SQLite;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data;
using System.Reflection;
namespace FrmBlog.Repositorys
{
    public enum DbType { 
    SqLite,MsSql
    }
    public  class BaseRepository
    {
        string _conStr;
        DbType _dbType;
        static DbConnection con;
        static DbTransaction _trans;
        public BaseRepository(string conString,DbType dbType)
        {
            _conStr = conString;
            _dbType = dbType;
            DataBaseInfo.DbType = _dbType;
            //con = null;            
        }
        protected DataTable GetDataTable(string sql)
        {
            DbConnection con = Connection;
            DbCommand dc = con.CreateCommand();
            dc.CommandText=sql;
            DbDataAdapter da = GetDbAdapter();
            da.SelectCommand = dc;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        protected T GetEntity<T>(DataRow row) where T : class, new()
        {
            T t = new T();
            //Determine whether the properties on the type exists for the record
            foreach (PropertyInfo propertyInfo in t.GetType().GetProperties())
            {
                
                if ((propertyInfo.PropertyType.Name==typeof(string).Name || !propertyInfo.PropertyType.IsClass) && row[propertyInfo.Name] != null)
                {
                    //Determine whether the type is the same,
                    //Easily assign the value
                    if (propertyInfo.PropertyType == row[propertyInfo.Name].GetType())
                    {
                        propertyInfo.SetValue(t, row[propertyInfo.Name], null);
                    }
                    else if(propertyInfo.PropertyType.IsEnum){
                        propertyInfo.SetValue(t, row[propertyInfo.Name], null);
                    }
                    else //We probably have to do some conversion
                    {
                        if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
                        {
                            object value = row[propertyInfo.Name];
                            if (value == DBNull.Value)
                            {
                                value = null;
                            }
                            propertyInfo.SetValue(t, value, null);
                        }
                    }
                }
            }
            return t;
        }
        protected object ExecuteScalar(string sql)
        {
            IDbConnection con = GetConnection();
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sql;        
            object obj = null;
            try
            {
                OpenConnectionIfClose();
                obj = cmd.ExecuteScalar();              

            }
            finally
            {
                CloseConnection();
            }
            return obj;
        }
        //public void BeginTransaction() {
        //    if (con == null)
        //        con = GetConnection();
        //    con.Open();
        //    _trans = con.BeginTransaction();
        //}
        //public void CommitTransaction()
        //{
        //    try
        //    {
        //        _trans.Commit();
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    catch (Exception exc)
        //    {                
        //        _trans.Rollback();
        //        if(con.State==ConnectionState.Open)
        //            con.Close();
        //        throw exc;
        //    }
          
        //}
        protected DbConnection GetConnection()
        {
            //if (con != null)
            //    return con;
            //string q = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            //if (_dbType == DbType.SqLite)
            //{
               
            //    string str=@"D:\www\customer\mayestro.net\db\FrmBlog.db;";
            //   _conStr = HttpContext.Current.Server.MapPath("/App_Data/FrmBlog.db");
            //    string sqlStr = string.Format("Data Source={0};Version=3;New=false;Compress=True;",str);
            //    con = new SQLiteConnection();
            //    con.ConnectionString = sqlStr;
            //    return con;
            //}
            //else
            //{
            //    con = new SqlConnection(q);
            //    return con;
            //}
            return Connection;
        }
        protected DbDataAdapter GetDbAdapter()
        {
          
            if (_dbType == DbType.SqLite)
            {
                return new SQLiteDataAdapter();
            }
            else
            {
                return new SqlDataAdapter();
            }
        }
        protected void ExecuteNonQuery(string sql)
        {
            IDbConnection con = Connection;
            IDbCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            try
            {
                if (!HasOpenTransaction() && con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                cmd.ExecuteNonQuery();
            }
            //catch { }
            finally
            {
                CloseConnection();
            }
        }
        public void OpenConnectionIfClose()
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
        }
        public bool HasOpenTransaction()
        {
            return ConnectionManager.Instance.HasOpenTransaction();
        }
        public DbConnection Connection
        {
            get
            {
                return ConnectionManager.Instance.GetConnection();
            }
        }
        public void BeginTransaction()
        {
            ConnectionManager.Instance.BeginTransaction();
        }
        public void CommitTransaction()
        {
            ConnectionManager.Instance.CommitTransaction();

        }
        public void CommitTransactionAndCloseSession()
        {
            try
            {
                ConnectionManager.Instance.CommitTransaction();
            }
            finally
            {
                ConnectionManager.Instance.CloseConnection();
            }
        }
        public void CloseConnection()
        {
            if(!HasOpenTransaction())
                ConnectionManager.Instance.CloseConnection();
        }
    }
}