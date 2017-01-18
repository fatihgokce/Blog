using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrmBlog.Models;
using System.Data;
using System.Text;
using FrmBlog.Helpers;
namespace FrmBlog.Repositorys
{
    public class RepositoryUser : BaseRepository, IRepository<User>
    {
        public RepositoryUser(string pathDatabase, DbType dbType)
            : base(pathDatabase, dbType)
        { }
        private User GetUser(IDataReader dr)
        {           
            User entity = new User();
            entity.UserId = int.Parse(dr["UserId"].ToString());
            entity.Email = dr["Email"].ToString();
            entity.Name = dr["Name"].ToString();
            entity.PicturePath = dr["PicturePath"].ToString();
            entity.DisplayName = dr["DisplayName"].ToString();
            entity.RecordDate = DateTime.Parse(dr["RecordDate"].ToString());
            entity.RoleId = int.Parse(dr["RoleId"].ToString());
            entity.Password = dr["Password"].ToString();
            return entity;
        }
        private User GetUser(DataRow dr)
        {
            User entity = new User();
            entity.UserId = int.Parse(dr["UserId"].ToString());
            entity.Email = dr["Email"].ToString();
            entity.Name = dr["Name"].ToString();
            entity.PicturePath = dr["PicturePath"].ToString();
            entity.DisplayName = dr["DisplayName"].ToString();
            entity.RecordDate = DateTime.Parse(dr["RecordDate"].ToString());
            entity.RoleId = int.Parse(dr["RoleId"].ToString());
            entity.Password = dr["Password"].ToString();
            return entity;
        }
        public User GetUserByEmail(string email)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from [User] where Email='{0}'", email.Trim());

            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            User entity = null;
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    entity = GetUser(dr);
                }

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    CloseConnection();
                    dr.Close();
                }
            }
            return entity;
        }
        public User GetUserByEmail(string email,string password)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from [User] where Email='{0}' and Password='{1}'", email.Trim(),password);

            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            User entity = null;
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    entity = GetUser(dr);
                }

            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    CloseConnection();
                    dr.Close();
                }
               
            }
            return entity;
        }
        #region IRepository<User> Members
        public void Insert(User item)
        {           
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("insert into [User](Email,Name,Password,RecordDate,RoleId,PicturePath)");
            sb.AppendFormat("values('{0}','{1}','{2}','{3}',{4},'{5}')",
                item.Email,item.Name,item.Password ,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),item.RoleId,item.PicturePath);

            IDbCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = sb.ToString();
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            //catch { }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }           
        }
        public void Update(User item)
        {
            throw new NotImplementedException();
        }
        public User GetById(long id)
        {
            //IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from [User] where UserId={0}", id);
            
            //IDbCommand cmd = con.CreateCommand();
            //cmd.CommandText = sb.ToString();
            //IDataReader dr = null;
            User entity = null;
            DataTable dt = GetDataTable(sb.ToString());
            if (dt.Rows.Count > 0)
                entity = GetUser(dt.Rows[0]);
            //try
            //{
            //    con.Open();
            //    dr = cmd.ExecuteReader();
            //    if (dr.Read())
            //    {
            //        entity =GetUser(dr);                  
            //    }

            //}
            //finally
            //{
            //    if (con.State == ConnectionState.Open)
            //    {
            //        con.Close();
            //        dr.Close();
            //    }
            //}
            return entity;
        }
        #endregion
        public List<User> GetListByDate(int maxRecord, int page, out int totalCount)
        {
            int offset = (page - 1) * maxRecord;
            string sql = "select * from [User] order by RecordDate desc limit {0} offset {1}".With(maxRecord, offset);
            object count = ExecuteScalar("select count(*) from [User]");
            totalCount = 0;
            if (count != null)
                totalCount = int.Parse(count.ToString());
            DataTable dt = GetDataTable(sql);
            List<User> liste = new List<User>();         
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                User entity = GetUser(dr);             
                liste.Add(entity);
            }
            return liste;
        }
        public bool IsRecordUser(string email)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
         
            sb.AppendFormat("select * from [User] where Email='{0}'",email);
        

            IDbCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {                 
                    string ema = dr["Email"].ToString();
                    if (!string.IsNullOrEmpty(ema))
                        return true;
                    else
                        return false;
                }
            }
            //catch { }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    CloseConnection();
                    dr.Close();
                }
              
            }
            return false;
        }
    }
}