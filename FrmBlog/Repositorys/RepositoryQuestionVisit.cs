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
    public class RepositoryQuestionVisit : BaseRepository, IRepository<QuestionVisit>
    {
        public RepositoryQuestionVisit(string pathDatabase, DbType dbType)
            : base(pathDatabase, dbType)
        { }
        #region IRepository<QuestionVisit> Members

        public void Insert(QuestionVisit item)
        {        
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA journal_mode = OFF;");
            sb.AppendFormat("insert into [QuestionVisit](QuestionId,UserId,Date,IPAddress,Referans)");
            sb.AppendFormat("values({0},{1},'{2}','{3}','{4}')",
                item.QuestionId, item.UserId, item.Date.ToString("yyyy-MM-dd HH:mm:ss"), item.IPAddress,item.Referans);

            IDbCommand cmd = con.CreateCommand();
            cmd.Connection = con;
            cmd.CommandText = sb.ToString();
            try
            {
                OpenConnectionIfClose();
                cmd.ExecuteNonQuery();
            }
            //catch { }
            finally
            {
                CloseConnection();
            }
        }

        public void Update(QuestionVisit item)
        {
            throw new NotImplementedException();
        }

        public QuestionVisit GetById(long id)
        {
            throw new NotImplementedException();
        }
        #endregion
        public bool IsBeforeConnected(string ip, DateTime date)
        {
            DataTable dt = GetDataTable("select * from [QuestionVisit] where IPAddress='{0}' and strftime('%Y-%m-%d',Date)='{1}'".With(ip,date.ToString("yyyy-MM-dd")));
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
        public bool IsBeforeConnectedWithIp(long QuestionId,DateTime today, string ip)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from [QuestionVisit] where QuestionId={0} and IPAddress='{1}' and strftime('%Y-%m-%d',Date)='{2}'",QuestionId, ip, today.ToString("yyyy-MM-dd"));


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
                    string id = dr["ID"].ToString();
                    if (!string.IsNullOrEmpty(id))
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
        public int VisitCount(long QuestionId)
        {
            IDbConnection con = GetConnection();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select count(*) from [QuestionVisit] where QuestionId={0}", QuestionId);

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
        public List<QuestionVisit> GetList(int maxRecord, int page, out int totalCount)
        {
            int offset = (page - 1) * maxRecord;
            string sql = "select * from QuestionVisit order by Date desc limit {0} offset {1}".With(maxRecord,offset);
            object count = ExecuteScalar("select count(*) from QuestionVisit order by Date desc");
            totalCount = 0;
            if (count != null)
                totalCount = int.Parse(count.ToString());
            DataTable dt = GetDataTable(sql);
            List<QuestionVisit> liste = new List<QuestionVisit>();
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                QuestionVisit entity = GetEntity(dr);               
                liste.Add(entity);
            }
            return liste;
        }
        public int ToplamTekilZiyaretciSayisi(DateTime date)
        {
            string str = "select count(distinct IPAddress) from [QuestionVisit] where strftime('%Y-%m-%d',Date)='{0}'".With(date.ToString("yyyy-MM-dd"));
            object obj = ExecuteScalar(str);
            if (obj != null)
                return int.Parse(obj.ToString());
            return 0;
        }
        public int ToplamZiyaretciSayisi(DateTime date)
        {
            string str = "select count(IPAddress) from [QuestionVisit] where strftime('%Y-%m-%d',Date)='{0}'".With(date.ToString("yyyy-MM-dd"));
            object obj = ExecuteScalar(str);
            if (obj != null)
                return int.Parse(obj.ToString());
            return 0;
        }
        private QuestionVisit GetEntity(DataRow dr)
        {
            QuestionVisit entity = new QuestionVisit();
            entity.QuestionId = int.Parse(dr["QuestionId"].ToString());
            entity.ID =int.Parse(dr["ID"].ToString());
            entity.IPAddress = dr["IPAddress"].ToString();
            entity.Date = DateTime.Parse(dr["Date"].ToString());
            entity.UserId = int.Parse(dr["UserId"].ToString());
            entity.Referans = dr["Referans"].ToString();
            return entity;
        }
        public void DeleteByIp(string ip)
        {
            ExecuteNonQuery("delete from QuestionVisit where IPAddress like '%{0}%'".With(ip));
        }
    }
}