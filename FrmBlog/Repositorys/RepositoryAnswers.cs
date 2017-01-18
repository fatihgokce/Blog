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
    public class RepositoryAnswers:BaseRepository,IRepository<Answer>
    {
        public RepositoryAnswers(string pathDatabase,DbType dbType)
            : base(pathDatabase,dbType)
        { }
        #region IRepository<Answers> Members

        public void Insert(Answer item)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("insert into [Answer](QuestionId,Reply,Date,IPAddress,BestReply,State,UserId)");
            sb.AppendFormat("values({0},'{1}','{2}','{3}','{4}',{5},{6})",
                item.QuestionId, item.Reply, item.Date.ToString("yyyy-MM-dd HH:mm:ss"), item.IPAddress,
                item.BestReply,(int)item.State,item.UserId);

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

        public void Update(Answer item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("update [Answer] set Reply='{0}',Date='{1}',IPAddress='{2}',BestReply='{3}',State={4},UserId={5} where AnswerId={6}",
             item.Reply, item.Date.ToString("yyyy-MM-dd HH:mm:ss"), item.IPAddress,
                item.BestReply, (int)item.State, item.UserId,item.AnswerId);
            ExecuteNonQuery(sb.ToString());
        }

        public Answer GetById(long id)
        {
            DataTable dt = GetDataTable("select * from Answer where AnswerId={0}".With(id));
            Answer ans = null;
            if (dt.Rows.Count > 0)
            {
                ans = GetData(dt.Rows[0]);
            }
            return ans;
        }        
        #endregion
       
        public void Delete(long id)
        {
            ExecuteNonQuery("delete from Answer where AnswerId={0}".With(id));
        }
        public int AnswerCountOfQuestion(long questionId)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select count(*) from [Answer] where QuestionId={0}", questionId);

            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            int cnt = 0;
            try
            {
                OpenConnectionIfClose();
                object obj = cmd.ExecuteScalar();
                if (obj != null)
                    cnt = int.Parse(obj.ToString());

            }
            finally
            {
                CloseConnection();
            }
            return cnt;
        }
        public List<Answer> GetListByQuestionId(long id)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from Answer where QuestionId={0}", id);
            IDbCommand cmd = con.CreateCommand();
            cmd.CommandText = sb.ToString();
            IDataReader dr = null;
            List<Answer> entitys = null;  
            try
            {
                OpenConnectionIfClose();
                dr = cmd.ExecuteReader();
                entitys = new List<Answer>();
                RepositoryUser repoUser = new RepositoryUser("",DbType.SqLite);
                while (dr.Read())
                {                    
                    entitys.Add(GetData(dr));
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
        public bool HaveBestReply(long questionId)
        {
            object obj = ExecuteScalar("select count(*) from Answer where QuestionId={0} and BestReply='1'".With(questionId));
            if (obj != null)
            {
                int i = int.Parse(obj.ToString());
                if (i == 0)
                    return false;
                else
                    return true;
            }
            return false;
        }
        public void SignBestReply(long answerId)
        {
            int quesId =int.Parse(ExecuteScalar("select QuestionId  from Answer where AnswerId={0}".With(answerId)).ToString());
            ExecuteNonQuery("update Answer set BestReply='{0}' where QuestionId={1}".With(0,quesId));
            ExecuteNonQuery("update Answer set BestReply='{0}' where AnswerId={1}".With(1,answerId));

        }
        private Answer GetData(dynamic dr)
        {
            Answer answer = new Answer();
            RepositoryUser repoUser = new RepositoryUser("", DbType.SqLite);
            answer.AnswerId = int.Parse(dr["AnswerId"].ToString());
            answer.QuestionId = int.Parse(dr["QuestionId"].ToString());
            answer.Reply = dr["Reply"].ToString();
            answer.Date = DateTime.Parse(dr["Date"].ToString());
            answer.IPAddress = dr["IPAddress"].ToString();
            answer.BestReply = bool.Parse(dr["BestReply"].ToString());
            answer.UserId =int.Parse( dr["UserId"].ToString());
            answer.User = repoUser.GetById(answer.UserId.Value);
            answer.State = (State)int.Parse(dr["State"].ToString());
            return answer;
        }
        public void DeleteByQuestionId(int questionId)
        {
            ExecuteNonQuery("delete from Answer where QuestionId={0}".With(questionId));
        }
    }
}