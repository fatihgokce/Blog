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
    public class RepositoryQuestion : BaseRepository, IRepository<Question>
    {
        public RepositoryQuestion(string pathDatabase, DbType dbType)
            : base(pathDatabase, dbType)
        { }
        #region IRepository<Question> Members

        public void Insert(Question item)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("insert into [Question](UserId,Title,Detail,Date,IPAddress,State)");
            sb.AppendFormat("values({0},'{1}','{2}','{3}','{4}',{5})",
                item.UserId, item.Title,item.Detail, item.Date.ToString("yyyy-MM-dd HH:mm:ss"), item.IPAddress,
                (int)item.State);

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

        public void Update(Question item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("update [Question] set UserId={0},Title='{1}',Detail='{2}',Date='{3}',IPAddress='{4}',State={5} where QuestionId={6}",
                item.UserId, item.Title, item.Detail, item.Date.ToString("yyyy-MM-dd HH:mm:ss"), item.IPAddress,
                (int)item.State,item.QuestionId);             
            ExecuteNonQuery(sb.ToString());
        }

        public Question GetById(long id)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from [Question] where QuestionId={0}", id);

            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            Question entity = null;
            //          CREATE TABLE [Question] (
            //[QuestionId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
            //[UserId] INTEGER NOT NULL, 
            //[Title] NVARCHAR(150), 
            //[Detail] NTEXT, 
            //[Date] DATETIME, 
            //[State] SMALLINT NOT NULL);
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                RepositoryUser repoUser = new RepositoryUser("", DbType.SqLite);
                if (dr.Read())
                {
                    entity = new Question();
                    entity.QuestionId = int.Parse(dr["QuestionId"].ToString());
                    entity.UserId = int.Parse(dr["UserId"].ToString());
                    entity.Title = dr["Title"].ToString();
                    entity.Detail = dr["Detail"].ToString();
                    entity.Date = DateTime.Parse(dr["Date"].ToString());
                    entity.State =(State)int.Parse(dr["State"].ToString());
                    entity.User = repoUser.GetById(entity.UserId);
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

        #endregion
        public int LastQuestionId()
        {
            object count = ExecuteScalar("select max(QuestionId) from Question");
            if (count != null)
              return int.Parse(count.ToString());
            return 0;
        }
        public List<Question> GetListByTagName(string tagName,int maxRecord, int page, out int totalCount)
        {
            int offset = (page - 1) * maxRecord;
            string sql = @"select distinct q.* from [Question] q inner join QuestionTag t on  q.QuestionId=t.QuestionId
inner join Tag tg on tg.[TagId]=t.[TagId]
where q.State={0} and tg.TagName= '{1}' order by Date desc limit {2} offset {3}".With((int)State.Yayinda,tagName, maxRecord, offset);
            object count = ExecuteScalar(@"select count(distinct q.[QuestionId]) from [Question] q inner join QuestionTag t on  q.QuestionId=t.QuestionId
inner join Tag tg on tg.[TagId]=t.[TagId] where q.State={0} and tg.TagName= '{1}'".With((int)State.Yayinda,tagName));
            totalCount = 0;
            if (count != null)
                totalCount = int.Parse(count.ToString());
            DataTable dt = GetDataTable(sql);
            List<Question> liste = new List<Question>();
            RepositoryUser repoUser = new RepositoryUser("", DbType.SqLite);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                Question entity = GetEntity(dr);
                entity.User = repoUser.GetById(entity.UserId);
                liste.Add(entity);
            }
            return liste;
        }
        public List<Question> GetAllListByDate(int maxRecord, int page, out int totalCount)
        {
            int offset = (page - 1) * maxRecord;
            string sql = "select * from Question  order by Date desc limit {0} offset {1}".With(maxRecord, offset);
            object count = ExecuteScalar("select count(*) from Question".With((int)State.Yayinda));
            totalCount = 0;
            if (count != null)
                totalCount = int.Parse(count.ToString());
            DataTable dt = GetDataTable(sql);
            List<Question> liste = new List<Question>();
            RepositoryUser repoUser = new RepositoryUser("", DbType.SqLite);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                Question entity = GetEntity(dr);
                entity.User = repoUser.GetById(entity.UserId);
                liste.Add(entity);
            }
            return liste;
        }
        public List<Question> GetListByDate(int maxRecord, int page,out int totalCount)
        {
            int offset = (page - 1) * maxRecord;
            string sql = "select * from Question where State={0}  order by Date desc limit {1} offset {2}".With((int)State.Yayinda,maxRecord, offset);
            //string sql = "select * from Question   order by Date desc limit {0} offset {1}".With(maxRecord, offset);
            object count = ExecuteScalar("select count(*) from Question where State={0}".With((int)State.Yayinda));
            totalCount = 0;
            if (count != null)
                totalCount = int.Parse(count.ToString());
            DataTable dt = GetDataTable(sql);
            List<Question> liste = new List<Question>();
            RepositoryUser repoUser = new RepositoryUser("", DbType.SqLite);
            for (int i = 0; i < dt.Rows.Count;i++)
            {
                DataRow dr = dt.Rows[i];
                Question entity = GetEntity<Question>(dr); //GetEntity(dr);//GetEntity<Question>(dr);               
                entity.User = repoUser.GetById(entity.UserId);
                liste.Add(entity);
            }
            return liste;
        }
        public List<Question> GetListQueryByDate(string q,int maxRecord, int page, out int totalCount)
        {
            int offset = (page - 1) * maxRecord;
            string sql = "select * from Question where State=1 and (Title like '%{0}%' or Detail like '%{0}%') order by Date desc limit {1} offset {2}".With(q,maxRecord, offset);
            object count = ExecuteScalar("select count(*) from Question where State=1 and (Title like '%{0}%' or Detail like '%{0}%')".With(q));
            totalCount = 0;
            if (count != null)
                totalCount = int.Parse(count.ToString());
            DataTable dt = GetDataTable(sql);
            List<Question> liste = new List<Question>();
            RepositoryUser repoUser = new RepositoryUser("", DbType.SqLite);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                Question entity = GetEntity(dr);
                entity.User = repoUser.GetById(entity.UserId);
                liste.Add(entity);
            }
            return liste;
        }
        public List<Question> GetListByTag(int count,params long[] tagIds)
        {
//            select q.* from Question q where q.[QuestionId] in (select QuestionId from QuestionTag
//where TagId=1 and QuestionId in(
//select QuestionId 
//from QuestionTag 
//where TagId=2) and QuestionId in(
//select QuestionId 
//from QuestionTag 
//where TagId=17))
//order by q.Date desc
            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct q.* from Question q where q.[QuestionId] in ( ");
            bool first = true;
            
            foreach (var t in tagIds)
            {
                if (first){
                    first = false; sql.AppendFormat(" select QuestionId from QuestionTag where TagId={0} ",t);                   
                }
                else
                {
                    sql.AppendFormat(" and QuestionId in( select QuestionId from QuestionTag where TagId={0})",t);
                }
            }
            sql.Append(") order by q.Date desc limit {0}".With(count));
         
            List<Question>liste = new List<Question>();
            DataTable dt = GetDataTable(sql.ToString());
            RepositoryUser repoUser = new RepositoryUser("", DbType.SqLite);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Question entity = GetEntity(dt.Rows[i]);//GetEntity<Question>(dt.Rows[i]);//
                entity.User = repoUser.GetById(entity.UserId);
                liste.Add(entity);
            }
              
            return liste;
        }
        public void SoruDurumunuDegistir(long questionId,State newState)
        {
            ExecuteNonQuery("update Question set State={0} where QuestionId={1}".With((int)newState,questionId));
        }
        public Question GetQuestionByAnswerId(long answerId)
        {
            DataTable dt = GetDataTable("select q.* from Question q inner join Answer a on q.QuestionId=a.QuestionId  where a.AnswerId={0}".With(answerId));
            Question ans = null;
            if (dt.Rows.Count > 0)
            {
                ans = GetEntity(dt.Rows[0]);
            }
            return ans;
        }
        public void DeleteByQuestionId(long questionId)
        {
            ExecuteNonQuery("delete from Question where QuestionId={0}".With(questionId));
        }
        private Question GetEntity(dynamic dr)
        {
            Question entity = new Question();
            entity.QuestionId = int.Parse(dr["QuestionId"].ToString());
            entity.UserId = int.Parse(dr["UserId"].ToString());
            entity.Title = dr["Title"].ToString();
            entity.Detail = dr["Detail"].ToString();
            entity.Date = DateTime.Parse(dr["Date"].ToString());
            entity.State = (State)int.Parse(dr["State"].ToString());
            return entity;
        }

    }
}