using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrmBlog.Models;
using FrmBlog.Helpers;
namespace FrmBlog.Repositorys
{
    public struct SettingKey
    {
        public const string SiteName = "SiteName";
        public const string PagerCount = "PagerCount";
    }
    public class RepositorySetting : BaseRepository
    {
        public RepositorySetting(string pathDatabase, DbType dbType)
            : base(pathDatabase,dbType)
        { }      
        public string this[string key]
        {
            get
            {
                object val = ExecuteScalar("select Value from Setting where Key='{0}' ".With(key));
                if (val != null)
                    return val.ToString();
                else
                    return string.Empty;
            }
            set
            {             
                object val = ExecuteScalar("select Value from Setting where Key='{0}' ".With(key));
                if (val != null)
                {
                    if (val.ToString().Trim() != value.Trim())
                    {
                        string sql = "PRAGMA journal_mode = OFF;update Setting set Value='{0}' where Key='{1}'".With(value, key);
                        ExecuteNonQuery(sql);
                    }
                }
                else
                {
                    string sql = "PRAGMA journal_mode = OFF;insert into Setting(Key,Value) values('{0}','{1}')".With(key, value);
                    ExecuteNonQuery(sql);
                }
            }
        }
    }
}