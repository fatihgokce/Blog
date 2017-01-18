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
    public class RepositoryQuestionTag : BaseRepository, IRepository<QuestionTag>
    {
        public RepositoryQuestionTag(string pathDatabase, DbType dbType)
            : base(pathDatabase, dbType)
        { }
        #region IRepository<QuestionTag> Members

        public void Insert(QuestionTag item)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("insert into [QuestionTag](TagId,QuestionId)");
            sb.AppendFormat("values({0},{1})",
                item.TagId, item.QuestionId);

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

        public void Update(QuestionTag item)
        {
            throw new NotImplementedException();
        }

        public QuestionTag GetById(long id)
        {
            throw new NotImplementedException();
        }
        #endregion
        public List<Tag> ListByQuestionId(long questionId)
        {
            string sql = "select t.* from Tag t inner join QuestionTag qt on t.[TagId]=qt.TagId inner join Question q on q.QuestionId=qt.QuestionId where q.[QuestionId]={0} and q.State=1".With(questionId);
            DataTable dt = GetDataTable(sql);
            List<Tag> liste = new List<Tag>();
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                DataRow dr=dt.Rows[i];
                Tag tag = new Tag();
                tag.TagId = int.Parse(dr["TagId"].ToString());
                tag.TagName = dr["TagName"].ToString();
                tag.RecordDate = DateTime.Parse(dr["RecordDate"].ToString());
                liste.Add(tag);
            }
            return liste;
        }
        public void Delete(long questionId, long tagId)
        {
            ExecuteNonQuery("delete from QuestionTag where QuestionId={0} and TagId={1}".With(questionId,tagId));
        }
        public void Delete(long questionId)
        {
            ExecuteNonQuery("delete from QuestionTag where QuestionId={0}".With(questionId));
        }
    }
}