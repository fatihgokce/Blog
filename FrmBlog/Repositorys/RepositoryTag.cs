using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FrmBlog.Models;
using System.Data;
using System.Text;
using FrmBlog.Helpers;
using FrmBlog.ModelData;
using System.Web.Caching;
namespace FrmBlog.Repositorys
{
    public class RepositoryTag : BaseRepository, IRepository<Tag>
    {
        public RepositoryTag(string pathDatabase, DbType dbType)
            : base(pathDatabase, dbType)
        { }
        public void Delete(Tag item)
        {
            string sql = "delete from Tag where TagId={0}".With(item.TagId);
            ExecuteNonQuery(sql);
        }
        #region IRepository<Tag> Members
        public void Insert(Tag item)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("insert into [Tag](TagName,RecordDate)");
            sb.AppendFormat("values('{0}','{1}')",
                item.TagName, item.RecordDate.ToString("yyyy-MM-dd HH:mm:ss"));

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

        public void Update(Tag item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("update Tag set TagName='{0}' where TagId={1}",item.TagName,item.TagId);
            ExecuteNonQuery(sb.ToString());
        }

        public Tag GetById(long id)
        {
            throw new NotImplementedException();
        }

        #endregion
        public Tag GetByName(string tagName)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from Tag where TagName='{0}'",tagName);
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            Tag tag = null;
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();               
                if (dr.Read())
                {
                    tag = new Tag();
                    tag.TagId = int.Parse(dr["TagId"].ToString());
                    tag.TagName = dr["TagName"].ToString();
                    tag.RecordDate = DateTime.Parse(dr["RecordDate"].ToString());                  
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
            return tag;
        }
        public List<TagInfo> GetTagInfoByQuery(int maxRecord,int page, string q,out int totalCount)
        {
            int offset = (page - 1) * maxRecord;
            string sqlWhere="";
            if (!string.IsNullOrEmpty(q))
                sqlWhere = " where TagName like '%{0}%' ".With(q);
            string sql = "select * from Tag {0} ".With(sqlWhere,maxRecord);
            object count = ExecuteScalar("select count(*) from Tag {0}".With(sqlWhere));
            totalCount = 0;
            if (count != null)
                totalCount = int.Parse(count.ToString());
            DataTable dt = GetDataTable(sql);
            List<TagInfo>liste=new List<TagInfo>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                TagInfo tag = new TagInfo();
                tag.TagName = dr["TagName"].ToString();
                tag.TagQuestionCount = GetQuestionCountByTagId(int.Parse(dr["TagId"].ToString()));
                liste.Add(tag);
            }
            return liste.OrderByDescending(x => x.TagQuestionCount).Skip(offset).Take(maxRecord).ToList();
        }
        public List<TagInfo> GetAllTagInfo(int count)
        {
            List<TagInfo> liste = (List<TagInfo>)HttpContext.Current.Cache.Get("tagInfoDataList");
            if(liste==null)
            {
                List<Tag> alltag = GetAll();
                liste = new List<TagInfo>();
                LoadTagInfo(liste, alltag);
                HttpContext.Current.Cache.Add("tagInfoDataList", liste, null, DateTime.Now.Add(new TimeSpan(0, 10, 0)), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }          
           
            return liste.OrderByDescending(x=>x.TagQuestionCount).Take(count).ToList();
        }

        private void LoadTagInfo(List<TagInfo> liste, List<Tag> alltag)
        {
            foreach (Tag item in alltag)
            {
                TagInfo inf = new TagInfo();
                inf.TagName = item.TagName;
                inf.TagQuestionCount = GetQuestionCountByTagId(item.TagId);
                liste.Add(inf);
            }
        }
        public int GetQuestionCountByTagId(long tagId)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select count(t.TagId)
from Tag t inner join QuestionTag qt on t.[TagId]=qt.TagId inner join Question q on q.QuestionId=qt.QuestionId
where qt.[TagId]={0} and q.State=1", tagId);

            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            int cnt = 0;
            try
            {
                OpenConnectionIfClose();
                object  obj = cmd.ExecuteScalar();
                if (obj != null)
                    cnt = int.Parse(obj.ToString());

            }
            finally
            {
                CloseConnection();
            }
            return cnt;
        }
        public List<string> GetListLikeTagName(string tagName)
        {
            DataTable dt = GetDataTable("select TagName from Tag where TagName like '%{0}%' ".With(tagName));
            List<string> liste = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                liste.Add(dt.Rows[i][0].ToString());
            }
            return liste;
        }
        public List<Tag> GetAll()
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from Tag");
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            List<Tag> entitys = null;
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                entitys = new List<Tag>();
                while (dr.Read())
                {
                    TagInsertList(dr, entitys);
                }

            }
            finally
            {
                CloseConnection();
            }
            return entitys;
        }

        private void TagInsertList(dynamic dr, List<Tag> entitys)
        {
            Tag tag = new Tag();
            tag.TagId = int.Parse(dr["TagId"].ToString());
            tag.TagName = dr["TagName"].ToString();
            tag.RecordDate = DateTime.Parse(dr["RecordDate"].ToString());
            entitys.Add(tag);
        }
        public List<Tag> GetListByQuestionId(long questionId)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"select t.*
from Tag t inner join QuestionTag qt on t.[TagId]=qt.TagId
where qt.[QuestionId]={0}", questionId);

            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            List<Tag> entitys = null;
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                entitys = new List<Tag>();
                while (dr.Read())
                {
                    Tag tag = new Tag();
                    tag.TagId = int.Parse(dr["TagId"].ToString());
                    tag.TagName = dr["TagName"].ToString();
                    tag.RecordDate = DateTime.Parse(dr["RecordDate"].ToString());
                   
                    entitys.Add(tag);
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
            return entitys;
        }
        
    }
}