using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Models
{
  //  CREATE TABLE [Question] (
  //[QuestionId] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  //[UserId] INTEGER NOT NULL, 
  //[Title] NVARCHAR(150), 
  //[Detail] NTEXT, 
  //[Date] DATETIME, 
  //[State] SMALLINT NOT NULL);
    public enum State { 
    Beklemede,Yayinda,Durduruldu
    }
    public class Question
    {
        public long QuestionId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime Date { get; set; }
        public State State { get; set; }
        public User User { get; set; }
        public string IPAddress { get; set; }
    }
}