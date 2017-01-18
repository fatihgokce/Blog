using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.EnterpriseServices;
using System.Data.Common;
using System.Configuration;
using System.Data.SQLite;
using System.Data.SqlClient;
namespace FrmBlog.Repositorys
{  
    public class ConnectionManager
    {
         public static ConnectionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
        }
        private ConnectionManager()
        {
            //InitSessionFactory();
        }
        private class Nested
        {
            static Nested() { }
            internal static readonly ConnectionManager NHibernateSessionManager =
                new ConnectionManager();
        }
        public DbConnection GetConnection()
        {
            if (ContextSession == null)
            {
                string q = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                if (DataBaseInfo.DbType == DbType.SqLite)
                {
                    string str = @"D:\www\customer\mayestro.net\db\FrmBlog.db;";
                    string lstr = HttpContext.Current.Server.MapPath("/App_Data/FrmBlog.db");
                    string sqlStr = string.Format("Data Source={0};Version=3;New=false;Compress=True;",lstr);
                    DbConnection con = new SQLiteConnection();
                    con.ConnectionString = sqlStr;
                    _connection = con;
                    ContextSession = con;
                    return con;
                }
                else
                {
                    DbConnection con = new SqlConnection(q);
                    _connection = con;
                    ContextSession = con;
                    return con;
                }
            }
            else
                return ContextSession;
        }
        public void BeginTransaction()
        {
            DbTransaction transaction = ContextTransaction;

            if (transaction == null)
            {
                if (System.Data.ConnectionState.Closed == GetConnection().State)
                    GetConnection().Open();
                transaction = GetConnection().BeginTransaction();
                ContextTransaction = transaction;
            }
        }
        public bool HasOpenTransaction()
        {
            DbTransaction transaction = ContextTransaction;

            return transaction != null;
        }
        public void CommitTransaction()
        {
            DbTransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {                   
                    transaction.Commit();
                    ContextTransaction = null;
                }
            }
           
            catch (Exception exc)
            {
                RollbackTransaction();
                ContextTransaction = null;
                throw exc;
            }
        }
        public void RollbackTransaction()
        {
            DbTransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Rollback();
                }

                ContextTransaction = null;
            }
            finally
            {
                CloseConnection();
            }
        }
        private DbTransaction ContextTransaction
        {
            get
            {
                return (DbTransaction)HttpContext.Current.Items[TRANSACTION_KEY];
            }
            set
            {              
                  HttpContext.Current.Items[TRANSACTION_KEY] = value;               
            }
        }
        private DbConnection ContextSession
        {
            get
            {
                return (DbConnection)HttpContext.Current.Items[SESSION_KEY];
            }
            set
            {
                HttpContext.Current.Items[SESSION_KEY] = value;
            }
        }
        public void CloseConnection()
        {
            DbConnection session = ContextSession;
            if (session != null && session.State==System.Data.ConnectionState.Open)
            {
                session.Close();
                ContextSession = null;
            }
        }
        private const string SESSION_KEY = "CONTEXT_SESSION";
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private DbConnection _connection;
    }
}