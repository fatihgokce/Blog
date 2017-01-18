using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrmBlog.Models;
using System.Data.SQLite;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Data;
namespace FrmBlog.Repositorys
{
    public class RepositoryRole:BaseRepository,IRepository<Role>
    {
        public RepositoryRole(string pathDatabase, DbType dbType)
            : base(pathDatabase,dbType)
        { }
        public Role GetByName(string roleName)
        {

            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from Role where Name='{0}'",roleName.Trim());
            
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            Role role = new Role();
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    role.RoleId = int.Parse(dr["RoleId"].ToString());
                    role.Name = dr["Name"].ToString();
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
            return role;
        }
        #region IRepository<Role> Members

        public void Insert(Role item)
        {
            throw new NotImplementedException();
        }

        public void Update(Role item)
        {
            throw new NotImplementedException();
        }

        public Role GetById(long id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}